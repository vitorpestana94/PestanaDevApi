using System.Data;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.UserQueries;
using Params = PestanaDevApi.Utils.DapperParams;
using PestanaDevApi.Models;
using Dapper;

namespace PestanaDevApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IDbConnectionFactory _factory;

        public UserRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<User?> GetUser(Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<User?>(Sql.GetUser, Params.ToUserId(userId));
        }
    }
}
