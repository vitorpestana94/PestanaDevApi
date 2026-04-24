using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;
using Sql = PestanaDevApi.Constants.Queries.LoginQueries;
using Params = PestanaDevApi.Utils.DapperParams;

namespace PestanaDevApi.Repositories
{
    public class LoginRepository: ILoginRepository
    {
        private readonly IDbConnectionFactory _factory;

        public LoginRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<User?> GetUserDataByEmail(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<User?>(Sql.SelectUserDataByEmail, Params.ToEmail(email));
        }

        public async Task<Guid> GetUserIdByEmail(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<Guid>(Sql.SelectUserIdByEmail, Params.ToEmail(email));
        }

        public async Task<Guid> GetUserIdByPlatformId(Platform platform, string platformId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<Guid>(Sql.SelectUserIdByPlatformId, 
            new 
            { 
                Platform = platform.ToString(), 
                PId = platformId 
            });
        }
    }
}
