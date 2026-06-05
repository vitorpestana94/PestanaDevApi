using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Extensions.Enums;
using Sql = PestanaDevApi.Constants.Queries.SignUpQueries;
using PestanaDevApi.Interfaces.Factories;

namespace PestanaDevApi.Repositories
{
    public class SignUpRepository: ISignUpRepository
    {
        private readonly IDbConnectionFactory _factory;

        public SignUpRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<User> RegisterUser(User user)
        {
            using IDbConnection db = _factory.CreateConnection();

            User newUser = user;

            newUser.Id = await db.ExecuteScalarAsync<Guid>(Sql.InsertUser, user.ToInsert());

            return newUser;
        }

        public async Task<User> RegisterUserByPlatform(User user)
        {
            using IDbConnection db = _factory.CreateConnection();

            User newUser = user;

            newUser.Id = await db.ExecuteScalarAsync<Guid>(Sql.InsertUserByPlatform, user.ToInsert());

            await InsertUserPlatformData(newUser.Id, (PlatformEnum)user.UserSignUpPlatform!, user.UserPlatformId!);

            return newUser;
        }

        public async Task InsertUserPlatformData(Guid userId, PlatformEnum platform, string platformId)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteScalarAsync<Guid>(Sql.InsertUserPlatformData, platform.ToInsert(userId, platformId));
        }

        public async Task<bool> IsEmailBeingUsed(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            Guid userId = await db.QueryFirstOrDefaultAsync<Guid>(Sql.SelectUserIdByEmail, new { Email = email });

            return userId != Guid.Empty;
        }
    }
}
