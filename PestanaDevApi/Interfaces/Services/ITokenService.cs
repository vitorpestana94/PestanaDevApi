using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ITokenService
    {

        /// <summary>
        /// It returns an object that contains both the JWT token and the refresh token.
        /// </summary>
        /// <returns>A Base64-encoded string representing the refresh token.</returns>
        Task<ApiToken> GenerateApiTokens(User user, string deviceId);

        /// <summary>
        /// Updates refresh tokens that are still valid within a specific time window.
        /// <para>
        /// This method updates the token and expiration date for records that have not yet
        /// exceeded the defined validity period (2 days since creation).
        /// </para>
        /// </summary>
        Task DeleteExpiredRefreshTokens();

        ApiToken GenerateApiTokensWithRefreshToken(User user, string refreshToken);
        Task<string?> GetAndUpdateRefreshToken(RefreshTokenRequestDto dto);
    }
}
