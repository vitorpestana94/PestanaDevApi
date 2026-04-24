using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Extensions;
using Sql = PestanaDevApi.Constants.Queries.TokenQueries;
using Params = PestanaDevApi.Utils.DapperParams;

namespace PestanaDevApi.Repositories
{
    public class TokenRepository: ITokenRepository
    {
        private readonly IDbConnectionFactory _factory;

        public  TokenRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task InsertOrUpdateRefreshToken(Guid userId, string deviceId, string refreshToken)
        {
            bool isSuccess;
            Guid existingRefreshToken = await GetRefreshTokenId(userId, deviceId);

            if (existingRefreshToken.IsEmpty()) 
            {
                isSuccess = await InsertRefreshToken(userId, deviceId, refreshToken);
            } 
            else 
            {
                isSuccess = await UpdateRefreshToken(userId, deviceId, refreshToken);
            }

            if (!isSuccess)
                throw new Exception();
        }

        public async Task DeleteExpiredRefreshTokens()
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.DeleteExpiredRefreshTokens);
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
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.UpdateRrefreshToken, Params.ToUpsertRefreshToken(userId, deviceId, token)) > 0;
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
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.InserRefreshToken,Params.ToUpsertRefreshToken(userId, deviceId, token)) > 0;
        }

        /// <summary>
        /// Inserts a new refresh token.
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deviceId">The unique identifier of the device. It's a UUID.</param>
        /// <returns>Returns an existing refresh token</returns>
        /// </summary>
        private async Task<Guid> GetRefreshTokenId(Guid userId, string deviceId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<Guid>(Sql.SelectRefreshToken, 
            new 
            { 
                UserId = userId, 
                DeviceId = deviceId 
            });
        }
        #endregion
    }
}
