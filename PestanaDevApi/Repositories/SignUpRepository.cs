using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Extensions.Enums;
using Sql = PestanaDevApi.Constants.Queries.SignUpQueries;

namespace PestanaDevApi.Repositories
{
    public class SignUpRepository: DefaultRepository, ISignUpRepository
    {
        public readonly IDbConnection _dbConnection;

        public SignUpRepository(IDbConnection dbConnection):base(dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task<User> RegisterUser(User user)
        {
            User newUser = user;

            newUser.Id = await _dbConnection.ExecuteScalarAsync<Guid>(Sql.InsertUser, user.ToInsert());

            return newUser;
        }

        public async Task<User> RegisterUserByPlatform(User user)
        {
            User newUser = user;

            newUser.Id = await _dbConnection.ExecuteScalarAsync<Guid>(Sql.InsertUserByPlatform, user.ToInsert());

            await InsertUserPlatformData(newUser.Id, (Platform)user.UserSignUpPlatform!, user.UserPlatformId!);

            return newUser;
        }

        public async Task InsertUserPlatformData(Guid userId, Platform platform, string platformId)
        {
            await _dbConnection.ExecuteScalarAsync<Guid>(Sql.InsertUserPlatformData, platform.ToInsert(userId, platformId));
        }

        public async Task<bool> IsEmailBeingUsed(string email)
        {
            Guid userId = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(Sql.SelectUserIdByEmail, new { Email = email });

            return userId != Guid.Empty;
        }
    }
}
