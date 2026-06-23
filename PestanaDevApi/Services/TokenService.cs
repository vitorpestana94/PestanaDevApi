using System.Security.Claims;
using System.Security.Cryptography;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using PestanaDevApi.Dtos.Requests;
using Consts = PestanaDevApi.Constants.ClaimsConstants;

namespace PestanaDevApi.Services
{
    public class TokenService: ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _config;

        public TokenService(ITokenRepository tokenRepository, IConfiguration config)
        {
            _tokenRepository = tokenRepository;
            _config = config;

            if (string.IsNullOrEmpty(_config["jwt.issuer"]))
                throw new InvalidOperationException("Issuer not configured! ");

            if (string.IsNullOrEmpty(_config["jwt.key"]))
                throw new InvalidOperationException("JWT key not configured!");
        }

        public async Task<ApiToken> GenerateApiTokens(User user, string deviceId)
        {
            return new ApiToken(CreateJwtToken(user), await CreatetRefreshToken(user.Id, deviceId));
        }

        public ApiToken GenerateApiTokensWithRefreshToken(User user, string refreshToken)
        {
            return new ApiToken(CreateJwtToken(user), refreshToken);
        }

        public async Task DeleteExpiredRefreshTokens()
        {
            await _tokenRepository.DeleteExpiredRefreshTokens();
        }

        public string GenerateResendConfirmationCodeJwt(ConfirmationCodeEmailRequestDto dto, string code)
        {
            return CreateJwtToken(dto, code);
        }

        public Task<string?> GetAndUpdateRefreshToken(RefreshTokenRequestDto dto)
        {
            return _tokenRepository.GetAndUpdateRefreshToken(dto, GenerateRefreshToken());
        }

        #region Private Methods
        /// <summary>
        /// It returns a base64-encoded string after persisting it in the database.
        /// </summary>
        /// <returns>A Base64-encoded string representing the refresh token.</returns>
        private async Task<string> CreatetRefreshToken(Guid userId, string deviceId)
        {
            string refreshToken = GenerateRefreshToken();

            await _tokenRepository.InsertOrUpdateRefreshToken(userId, deviceId, refreshToken);

            return refreshToken;
        }

        /// <summary>
        /// Generates a secure random refresh token.
        /// </summary>
        /// <returns>A Base64-encoded string representing the refresh token.</returns>
        private static string GenerateRefreshToken()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] randomBytes = new byte[32];
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Generates a JWT access token for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A JWT token as a string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the JWT signing key is not configured.</exception>
        private string CreateJwtToken(User user)
        {
            return new JsonWebTokenHandler().CreateToken(GetTokenDescriptor(GetTokenClaims(user)));
        }

        /// <summary>
        /// Generates a JWT access token for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A JWT token as a string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the JWT signing key is not configured.</exception>
        private string CreateJwtToken(ConfirmationCodeEmailRequestDto dto, string code)
        {
            return new JsonWebTokenHandler().CreateToken(GetTokenDescriptor(GetTokenClaims(dto, code), expires: DateTime.UtcNow.AddMinutes(3)));
        }

        /// <summary>
        /// Generates a JWT claims.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A JWT claims.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the JWT Issuer is not configured.</exception>
        private List<Claim> GetTokenClaims(User user)
        {
            return
            [
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iss, _config["jwt.issuer"]!),
                new(JwtRegisteredClaimNames.Email, user.UserEmail),
                new(JwtRegisteredClaimNames.Name, user.UserName),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            ];
        }

        /// <summary>
        /// Generates a JWT claims.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A JWT claims.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the JWT Issuer is not configured.</exception>
        private List<Claim> GetTokenClaims(ConfirmationCodeEmailRequestDto dto, string code)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return
            [
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iss, _config["jwt.issuer"]!),
                new(JwtRegisteredClaimNames.Email, dto.ClientEmail),
                new(JwtRegisteredClaimNames.Locale, dto.ClientLocale),
                new(JwtRegisteredClaimNames.Sub, dto.ClientEmail),
                new(Consts.Code, code),
                new(Consts.Purpose, Consts.EmailConfirmation)
            ];
        }

        /// <summary>
        /// Generates Token description.
        /// </summary>
        /// <param name="claims">The list of JWT'S claims.</param>
        /// <returns>A JWT token descriptor.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the JWT Key is not configured.</exception>
        private SecurityTokenDescriptor GetTokenDescriptor(List<Claim> claims, DateTime? expires = null)
        {
            return new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires.HasValue ? expires : DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(_config["jwt.key"]!)), SecurityAlgorithms.HmacSha256),
            };
        }
        #endregion
    }
}
