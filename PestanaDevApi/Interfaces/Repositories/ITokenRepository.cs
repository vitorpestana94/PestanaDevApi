using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ITokenRepository
    {

        /// <summary>
        /// It inserts a new refresh token if there's not an existing one or update it otherwise.
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deviceId">The unique identifier of the device. It's a UUID.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// </summary>
        Task InsertOrUpdateRefreshToken(Guid userId, string deviceId, string refreshToken);

        /// <summary>
        /// Updates refresh tokens that are still valid within a specific time window.
        /// <para>
        /// This method updates the token and expiration date for records that have not yet
        /// exceeded the defined validity period (2 days since creation).
        /// </para>
        /// </summary>
        Task DeleteExpiredRefreshTokens();

        Task<string?> GetAndUpdateRefreshToken(RefreshTokenRequestDto dto, string refreshToken);
    }
}
