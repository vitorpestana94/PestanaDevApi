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
        Task<ApiToken> GenerateApiTokens(User user);

        /// <summary>
        /// Updates refresh tokens that are still valid within a specific time window.
        /// <para>
        /// This method updates the token and expiration date for records that have not yet
        /// exceeded the defined validity period (2 days since creation).
        /// </para>
        /// </summary>
        Task DeleteExpiredRefreshTokens();
        ApiToken GenerateApiTokensWithRefreshToken(User user, string deviceId, string refreshToken);
        Task<string?> GetAndUpdateRefreshToken(RefreshTokenRequestDto dto);
        string GetDeviceId();

        /// <summary>
        /// Generates a short lived token (10 minutes) used only to connect to the SignalR hubs.
        /// It has its own audience (PestanaDevApi.Hub), so the main endpoints reject it, and it only carries
        /// the user id (sub), the same one of the main JWT.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the authenticated user.
        /// </param>
        /// <returns>
        /// The hub token.
        /// </returns>
        string GenerateHubToken(Guid userId);
    }
}
