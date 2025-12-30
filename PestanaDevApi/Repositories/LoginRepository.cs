using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;



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
                  id, user_name, user_email, user_password, 
                  user_picture, user_signup_platform
            FROM
                  USERS_PROFILE_DATA
            WHERE
                  user_email = @Email", new { Email = email});
        }
    }
}
