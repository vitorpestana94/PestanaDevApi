using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Extensions.Dtos.Requests;
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

        public async Task InsertRefreshToken(Guid userId, string deviceId, string refreshToken)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.InserRefreshToken, Params.ToInsertRefreshToken(userId, deviceId, refreshToken));
        }

        public async Task DeleteExpiredRefreshTokens()
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.DeleteExpiredRefreshTokens);
        }

        public async Task<string?> UpdateRefreshToken(RefreshTokenRequestDto dto, string refreshToken)
        {
            using IDbConnection db = _factory.CreateConnection();

            if (await db.ExecuteAsync(Sql.UpdateRefreshToken, dto.ToUpdateDeviceIdAndRefreshToken(refreshToken)) == 0)
                return null;

            return refreshToken;
        }
    }
}
