using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;

namespace PestanaDevApi.Repositories
{
    public class TokenRepository : DefaultRepository, ITokenRepository
    {
        private readonly IDbConnection _dbConnection;

        public  TokenRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// It inserts a new refresh token if there's not an existing one or update it otherwise.
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deviceId">The unique identifier of the device. It's a UUID.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// </summary>
        public async Task InsertOrUpdateRefreshToken(Guid userId, string deviceId, string refreshToken)
        {
            bool isSuccess;
            Guid existingRefreshToken = await GetRefreshTokenId(userId, deviceId);

            if (existingRefreshToken == Guid.Empty) 
            {
                isSuccess = await InsertRefreshToken(userId, deviceId, refreshToken);
            } 
            else 
            {
                isSuccess = await UpdateRefreshToken(userId, deviceId, refreshToken); ;
            }

            if (!isSuccess)
                throw new Exception();
        }

        #region Private Methods
        /// <summary>
        /// Updates a refresh token.
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deviceId">The unique identifier of the device. It's a UUID.</param>
        /// <param name="token">The refresh token.</param>
        /// <returns>A boolean representing if the operation was succesfull.</returns>
        /// </summary>
        private async Task<bool> UpdateRefreshToken(Guid userId, string deviceId, string token)
        {
            return  await _dbConnection.ExecuteAsync(@"
            UPDATE 
                  REFRESH_TOKEN 
            SET    
                  token = @Token, 
                  expired_at = @ExpiredAt 
            WHERE 
                  user_profile_id = @userId 
            AND 
                  device_id = @deviceId", 
            new { Token = token, ExpiredAt = DateTime.UtcNow.AddDays(2), UserId = userId, DeviceId = deviceId }) > 0;
        }

        /// <summary>
        /// Inserts a new refresh token.
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deviceId">The unique identifier of the device. It's a UUID.</param>
        /// <param name="token">The refresh token.</param>
        /// <returns>A boolean representing if the operation was succesfull.</returns>
        /// </summary>
        private async Task<bool> InsertRefreshToken(Guid userId, string deviceId, string token)
        {
            return await _dbConnection.ExecuteAsync(@"
            INSERT INTO REFRESH_TOKEN (user_profile_id, device_id, token, expired_at) 
            VALUES (@UserId, @DeviceId, @Token, @ExpiredAt)", 
            new { UserId = userId, DeviceId = deviceId, Token = token, ExpiredAt = DateTime.UtcNow.AddDays(2) }) > 0;
        }

        /// <summary>
        /// Inserts a new refresh token.
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deviceId">The unique identifier of the device. It's a UUID.</param>
        /// <returns>Returns an existing refresh token</returns>
        /// </summary>
        private async Task<Guid> GetRefreshTokenId(Guid userId, string deviceId)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<Guid>(@"
            SELECT 
                  id 
            FROM 
                   REFRESH_TOKEN 
            WHERE 
                   user_profile_id = @UserId 
            AND 
                   device_id = @DeviceId", 
            new { UserId = userId, DeviceId = deviceId });
        }
        #endregion
    }
}
