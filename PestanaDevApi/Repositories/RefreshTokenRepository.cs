using System.Data;
using Dapper;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Extensions.Dtos.Requests;
using Sql = PestanaDevApi.Constants.Queries.RefreshTokenQueries;

namespace PestanaDevApi.Repositories
{
    public class RefreshTokenRepository: IRefreshTokenRepository
    {
        private readonly IDbConnectionFactory _factory;

        public RefreshTokenRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Retrieves user data associated with a specific refresh token.
        /// </summary>
        /// <param name="request">DTO containing the refresh token string.</param>
        /// <returns>The <see cref="User"/> entity if the token is valid; otherwise, null.</returns>
        public async Task<User?> GetUserDataByRefreshToken(RefreshTokenRequestDto request)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<User?>(Sql.SelectUserDataByRefreshToken, request.ToSelectByRefreshToken());
        }
    }
}
