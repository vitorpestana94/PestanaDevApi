using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PestanaDevApi.AppConfig;
using PestanaDevApi.Constants;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Interfaces.Services.Auth;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Services;
using PestanaDevApi.Services.Auth;

namespace PestanaDevApi.Tests.Services
{
    public class PlatformServiceTests
    {
        private readonly Mock<IPlatformAuthService> _platformAuthService = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly PlatformService _service;

        public PlatformServiceTests()
        {
            _service = new(_platformAuthService.Object, _tokenService.Object);
        }

        [Fact]
        public async Task LoginOrSignUpWithProvider_ReturnsForbidden_WhenTokenIsNotValid()
        {
            AuthResponseDto response = await _service.LoginOrSignUpWithProvider(new() { Platform = PlatformEnum.Google, Token = "invalid" });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task LoginOrSignUpWithProvider_ReturnsTokens_WhenTokenIsValid()
        {
            _platformAuthService.Setup(p => p.GetUserByIoken("valid", PlatformEnum.Google)).ReturnsAsync(new User());
            _tokenService.Setup(t => t.GenerateApiTokens(It.IsAny<User>())).ReturnsAsync(new ApiToken("jwt", "refresh"));

            AuthResponseDto response = await _service.LoginOrSignUpWithProvider(new() { Platform = PlatformEnum.Google, Token = "valid" });

            Assert.True(response.IsSuccess);
        }
    }

    public class PlatformAuthServiceTests
    {
        private readonly Mock<ILoginRepository> _loginRepository = new();
        private readonly Mock<ISignUpRepository> _signUpRepository = new();
        private readonly Mock<IGoogleAuthService> _googleAuthService = new();
        private readonly PlatformAuthService _service;
        private readonly GoogleJsonWebSignature.Payload _googlePayload = new() { Subject = "google-id", Email = "user@email.com", Name = "User" };

        public PlatformAuthServiceTests()
        {
            _service = new(_loginRepository.Object, new Mock<ILinkedinAuthService>().Object, new Mock<IGitHubAuthService>().Object, _signUpRepository.Object, _googleAuthService.Object);
        }

        [Fact]
        public async Task GetUserByIoken_ReturnsNull_WhenGoogleTokenIsNotValid()
        {
            Assert.Null(await _service.GetUserByIoken("invalid", PlatformEnum.Google));
        }

        [Fact]
        public async Task GetUserByIoken_LinksThePlatform_WhenEmailIsAlreadyRegistered()
        {
            Guid userId = Guid.NewGuid();
            _googleAuthService.Setup(g => g.ValidateGoogleToken("valid")).ReturnsAsync(_googlePayload);
            _loginRepository.Setup(l => l.GetUserIdByEmail(_googlePayload.Email)).ReturnsAsync(userId);

            User? user = await _service.GetUserByIoken("valid", PlatformEnum.Google);

            Assert.Equal(userId, user!.Id);
            _signUpRepository.Verify(s => s.InsertUserPlatformData(userId, PlatformEnum.Google, "google-id"), Times.Once);
            _signUpRepository.Verify(s => s.RegisterUserByPlatform(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task GetUserByIoken_RegistersTheUser_WhenItIsANewUser()
        {
            _googleAuthService.Setup(g => g.ValidateGoogleToken("valid")).ReturnsAsync(_googlePayload);
            _signUpRepository.Setup(s => s.RegisterUserByPlatform(It.IsAny<User>())).ReturnsAsync(new User { UserEmail = _googlePayload.Email });

            User? user = await _service.GetUserByIoken("valid", PlatformEnum.Google);

            Assert.Equal(_googlePayload.Email, user!.UserEmail);
            _signUpRepository.Verify(s => s.RegisterUserByPlatform(It.Is<User>(u => u.UserPlatformId == "google-id")), Times.Once);
        }
    }

    public class TokenServiceTests
    {
        private readonly Mock<ITokenRepository> _repository = new();

        private static IConfiguration Config(string? issuer = "PestanaDevApi", string? key = null) =>
            new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["jwt.issuer"] = issuer,
                ["jwt.key"] = key ?? Convert.ToBase64String(new byte[32])
            }).Build();

        [Fact]
        public void Constructor_Throws_WhenIssuerIsNotConfigured()
        {
            Assert.Throws<InvalidOperationException>(() => new TokenService(_repository.Object, Config(issuer: "")));
        }

        [Fact]
        public async Task GenerateApiTokens_CreatesJwtWithUserClaimsAndStoresRefreshToken()
        {
            User user = new() { Id = Guid.NewGuid(), UserName = "User", UserEmail = "user@email.com" };

            ApiToken tokens = await new TokenService(_repository.Object, Config()).GenerateApiTokens(user);

            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokens.Token);
            Assert.Equal(user.Id.ToString(), jwt.Subject);
            Assert.Equal("user@email.com", jwt.Claims.First(c => c.Type == "email").Value);
            Assert.Equal([TokenConstants.ApiAudience], jwt.Audiences);
            _repository.Verify(r => r.InsertRefreshToken(user.Id, It.IsAny<string>(), tokens.RefreshToken), Times.Once);
        }

        #region Hub Token
        private static readonly string Key = Convert.ToBase64String(new byte[32]);

        private static TokenValidationParameters ValidationParameters(string audience) => new()
        {
            ValidIssuer = "PestanaDevApi",
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Key))
        };

        [Fact]
        public void GenerateHubToken_HasHubAudienceSameSubShortLifeAndNoUserData()
        {
            Guid userId = Guid.NewGuid();

            string token = new TokenService(_repository.Object, Config(key: Key)).GenerateHubToken(userId);

            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            Assert.Equal([TokenConstants.HubAudience], jwt.Audiences);
            Assert.Equal(userId.ToString(), jwt.Subject);
            Assert.True(jwt.ValidTo <= DateTime.UtcNow.AddMinutes(TokenConstants.HubTokenExpirationMinutes).AddSeconds(5));
            Assert.True(jwt.ValidTo > DateTime.UtcNow.AddMinutes(TokenConstants.HubTokenExpirationMinutes - 1));
            Assert.DoesNotContain(jwt.Claims, c => c.Type is "email" or "name" or "sid");
        }

        [Fact]
        public async Task HubToken_IsOnlyValidForTheHubAudience()
        {
            TokenService service = new(_repository.Object, Config(key: Key));
            JsonWebTokenHandler handler = new();

            string hubToken = service.GenerateHubToken(Guid.NewGuid());
            string mainToken = (await service.GenerateApiTokens(new User { Id = Guid.NewGuid() })).Token;

            Assert.True((await handler.ValidateTokenAsync(hubToken, ValidationParameters(TokenConstants.HubAudience))).IsValid);
            Assert.False((await handler.ValidateTokenAsync(mainToken, ValidationParameters(TokenConstants.HubAudience))).IsValid);
            Assert.False((await handler.ValidateTokenAsync(hubToken, ValidationParameters(TokenConstants.ApiAudience))).IsValid);
        }

        [Fact]
        public void IsApiAudience_RejectsHubTokens()
        {
            Assert.True(JwtConfig.IsApiAudience([TokenConstants.ApiAudience]));
            Assert.True(JwtConfig.IsApiAudience([]));   // main tokens issued before the audience existed
            Assert.False(JwtConfig.IsApiAudience([TokenConstants.HubAudience]));
            Assert.False(JwtConfig.IsApiAudience([TokenConstants.ApiAudience, TokenConstants.HubAudience]));
        }
        #endregion
    }

    public class LoginServiceHubTokenTests
    {
        [Fact]
        public void GenerateHubToken_ReturnsTheTokenWithTheApiResponsePattern()
        {
            Guid userId = Guid.NewGuid();
            Mock<ITokenService> tokenService = new();
            tokenService.Setup(t => t.GenerateHubToken(userId)).Returns("hub-token");
            LoginService service = new(new Mock<ILoginRepository>().Object, tokenService.Object, new Mock<IPlatformAuthService>().Object, new Mock<ICaptchaService>().Object);

            HubTokenResponseDto response = service.GenerateHubToken(userId);

            Assert.True(response.IsSuccess);
            Assert.Equal("hub-token", response.Token);
            Assert.Equal("Ok", response.Message);
        }
    }
}
