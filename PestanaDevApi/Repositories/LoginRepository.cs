using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Repositories
{
    public class LoginRepository: DefaultRepository, ILoginRepository
    {
        public readonly IDbConnection _dbConnection;

        public LoginRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User?> GetUserDataByEmail(string email)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<User?>(@"
            SELECT
                  id, user_name, user_email, user_password, user_picture 
            FROM
                  USERS_PROFILE_DATA
            WHERE
                  user_email = @Email", 
            new { Email = email});
        }

        public async Task<Guid> GetUserIdEmail(string email)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<Guid>(@"
            SELECT
                  id 
            FROM
                  USERS_PROFILE_DATA
            WHERE
                  user_email = @Email",
            new { Email = email });
        }

        public async Task<Guid> GetUserIdByPlatformId(Platform platform, string platformId)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<Guid>(@"
            SELECT
                  user_id
            FROM
                  USERS_PROFILE_PLATFORM_DATA
            WHERE
                  user_signup_platform = @Platform
            AND   
                  user_platform_id = @PId;", 
            new { Platform = platform.ToString(), PId = platformId });
        }
    }
}
