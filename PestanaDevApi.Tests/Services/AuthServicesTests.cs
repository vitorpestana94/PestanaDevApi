using System.Net;
using Moq;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Interfaces.Services.Auth;
using PestanaDevApi.Interfaces.Utils;
using PestanaDevApi.Models;
using PestanaDevApi.Services;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Tests.Services
{
    public class LoginServiceTests
    {
        private readonly Mock<ILoginRepository> _repository = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly Mock<ICaptchaService> _captchaService = new();
        private readonly LoginService _service;

        public LoginServiceTests()
        {
            _captchaService.Setup(c => c.ValidateCaptchaV3(It.IsAny<string>())).ReturnsAsync(true);
            _tokenService.Setup(t => t.GenerateApiTokens(It.IsAny<User>())).ReturnsAsync(new ApiToken("jwt", "refresh"));
            _service = new(_repository.Object, _tokenService.Object, new Mock<IPlatformAuthService>().Object, _captchaService.Object);
        }

        private static LoginRequestDto Request(string password = "myPassword") => new() { Email = "user@email.com", Password = password, CaptchaToken = "token" };

        [Fact]
        public async Task Login_ReturnsForbidden_WhenCaptchaIsNotHuman()
        {
            _captchaService.Setup(c => c.ValidateCaptchaV3(It.IsAny<string>())).ReturnsAsync(false);

            AuthResponseDto response = await _service.Login(Request());

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist()
        {
            AuthResponseDto response = await _service.Login(Request());

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsForbidden_WhenUserSignedUpByPlatform()
        {
            _repository.Setup(r => r.GetUserDataByEmail(It.IsAny<string>())).ReturnsAsync(new User { SignupByPlatform = true });

            AuthResponseDto response = await _service.Login(Request());

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsWrong()
        {
            _repository.Setup(r => r.GetUserDataByEmail(It.IsAny<string>())).ReturnsAsync(new User { UserPassword = HashFactory.HashPassword("myPassword") });

            AuthResponseDto response = await _service.Login(Request(password: "wrong"));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsTokens_WhenCredentialsAreValid()
        {
            _repository.Setup(r => r.GetUserDataByEmail(It.IsAny<string>())).ReturnsAsync(new User { UserPassword = HashFactory.HashPassword("myPassword") });

            AuthResponseDto response = await _service.Login(Request());

            Assert.True(response.IsSuccess);
            Assert.Equal("jwt", response.ApiTokens!.Token);
        }
    }

    public class SignUpServiceTests
    {
        private readonly Mock<ISignUpRepository> _repository = new();
        private readonly Mock<IConfirmedEmailsRepository> _confirmedEmails = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly SignUpService _service;
        private readonly SignUpRequestDto _request = new() { Email = "user@email.com", Name = "User", Password = "myPassword" };

        public SignUpServiceTests()
        {
            _service = new(_repository.Object, _tokenService.Object, _confirmedEmails.Object);
        }

        [Fact]
        public async Task SignUp_ReturnsBadRequest_WhenEmailIsInvalid()
        {
            SignUpResponseDto response = await _service.SignUp(new() { Email = "invalid", Name = "User", Password = "x" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignUp_ReturnsBadRequest_WhenEmailIsBeingUsed()
        {
            _repository.Setup(r => r.IsEmailBeingUsed(_request.Email)).ReturnsAsync(true);

            SignUpResponseDto response = await _service.SignUp(_request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignUp_ReturnsForbidden_WhenEmailIsNotConfirmed()
        {
            SignUpResponseDto response = await _service.SignUp(_request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task SignUp_RegistersUserAndDeletesConfirmation_WhenEmailIsConfirmed()
        {
            _confirmedEmails.Setup(c => c.IsEmailConfirmed(_request.Email)).ReturnsAsync(true);
            _repository.Setup(r => r.RegisterUser(It.IsAny<User>())).ReturnsAsync(new User());
            _tokenService.Setup(t => t.GenerateApiTokens(It.IsAny<User>())).ReturnsAsync(new ApiToken("jwt", "refresh"));

            SignUpResponseDto response = await _service.SignUp(_request);

            Assert.True(response.IsSuccess);
            _confirmedEmails.Verify(c => c.DeleteEmailConfirmation(_request.Email), Times.Once);
        }
    }

    public class ForgotPasswordServiceTests
    {
        private readonly Mock<IConfirmedEmailsRepository> _confirmedEmails = new();
        private readonly Mock<IForgotPasswordRepository> _repository = new();
        private readonly ForgotPasswordService _service;

        public ForgotPasswordServiceTests()
        {
            _service = new(_confirmedEmails.Object, _repository.Object);
        }

        [Fact]
        public async Task ForgotPassword_ReturnsBadRequest_WhenEmailIsNotConfirmed()
        {
            ForgotPasswordResponseDto response = await _service.ForgotPassword(new() { Email = "user@email.com", NewPassword = "new" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _repository.Verify(r => r.ChangeUserPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ForgotPassword_SavesHashedPassword_WhenEmailIsConfirmed()
        {
            _confirmedEmails.Setup(c => c.IsEmailConfirmed("user@email.com")).ReturnsAsync(true);

            ForgotPasswordResponseDto response = await _service.ForgotPassword(new() { Email = "user@email.com", NewPassword = "new" });

            Assert.True(response.IsSuccess);
            _repository.Verify(r => r.ChangeUserPassword("user@email.com", It.Is<string>(hash => hash != "new")), Times.Once);
            _confirmedEmails.Verify(c => c.DeleteEmailConfirmation("user@email.com"), Times.Once);
        }
    }

    public class RefreshTokenServiceTests
    {
        private readonly Mock<IRefreshTokenRepository> _repository = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly RefreshTokenService _service;
        private readonly RefreshTokenRequestDto _request = new() { UserId = Guid.NewGuid(), Token = "token", DeviceId = "device" };

        public RefreshTokenServiceTests()
        {
            _service = new(_repository.Object, _tokenService.Object);
        }

        [Fact]
        public async Task RefreshToken_ReturnsUnauthorized_WhenTokenIsNotValid()
        {
            AuthResponseDto response = await _service.RefreshToken(_request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_ReturnsNewTokens_WhenTokenIsValid()
        {
            _repository.Setup(r => r.GetUserDataByRefreshToken(_request)).ReturnsAsync(new User());
            _tokenService.Setup(t => t.GetAndUpdateRefreshToken(_request)).ReturnsAsync("newRefresh");
            _tokenService.Setup(t => t.GenerateApiTokensWithRefreshToken(It.IsAny<User>(), "device", "newRefresh")).Returns(new ApiToken("jwt", "newRefresh"));

            AuthResponseDto response = await _service.RefreshToken(_request);

            Assert.True(response.IsSuccess);
            Assert.Equal("newRefresh", response.ApiTokens!.RefreshToken);
        }
    }

    public class CaptchaServiceTests
    {
        private readonly Mock<IGoogleApisHttpClient> _googleClient = new();

        [Theory]
        [InlineData(0.9, true)]
        [InlineData(0.1, false)]
        public async Task ValidateCaptchaV3_UsesGoogleScore(double score, bool expected)
        {
            _googleClient.Setup(g => g.GetCaptchaV3Validation(It.IsAny<string>())).ReturnsAsync(new GetCaptchaV3ValidationResponse(new GoogleCaptchaV3ValidationResponseDto { Score = score }));

            Assert.Equal(expected, await new CaptchaService(_googleClient.Object).ValidateCaptchaV3("token"));
        }

        [Fact]
        public async Task ValidateCaptchaV3_ReturnsFalse_WhenGoogleDoesNotRespond()
        {
            _googleClient.Setup(g => g.GetCaptchaV3Validation(It.IsAny<string>())).ReturnsAsync(new GetCaptchaV3ValidationResponse(HttpStatusCode.InternalServerError));

            Assert.False(await new CaptchaService(_googleClient.Object).ValidateCaptchaV3("token"));
        }
    }

    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repository = new();
        private readonly Mock<IConfirmedEmailsRepository> _confirmedEmails = new();
        private readonly UserService _service;
        private readonly Guid _userId = Guid.NewGuid();

        public UserServiceTests()
        {
            _service = new(_repository.Object, new Mock<ISignUpRepository>().Object, _confirmedEmails.Object);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            GetUserResponseDto response = await _service.GetUser(_userId);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangeUserData_ReturnsBadRequest_WhenNothingChanged()
        {
            ChangeUserDataResponseDto response = await _service.ChangeUserData(new(), _userId);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ChangeUserPassword_ReturnsUnauthorized_WhenCurrentPasswordIsWrong()
        {
            _repository.Setup(r => r.GetUser(_userId)).ReturnsAsync(new User { UserPassword = HashFactory.HashPassword("current") });

            ChangePasswordResponseDto response = await _service.ChangeUserPassword(new() { CurrentPassword = "wrong", NewPassword = "new" }, _userId);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            _repository.Verify(r => r.ChangeUserPassword(It.IsAny<ChangePasswordRequestDto>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_ReturnsBadRequest_WhenEmailIsNotConfirmed()
        {
            _repository.Setup(r => r.GetUser(_userId)).ReturnsAsync(new User { UserEmail = "user@email.com" });

            DeleteUserResponseDto response = await _service.DeleteUser(_userId);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _repository.Verify(r => r.DeleteUserData(It.IsAny<Guid>()), Times.Never);
        }
    }
}
