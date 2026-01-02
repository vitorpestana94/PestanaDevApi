using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

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

            newUser.Id = await _dbConnection.ExecuteScalarAsync<Guid>(@"
            INSERT INTO USERS_PROFILE_DATA (user_name, user_email, user_password, user_picture)
            VALUES (@Name, @Email, @Password, @Picture)
            RETURNING id;", 
            new
            {
                Name = user.UserName,
                Email = user.UserEmail,
                Password = user.UserPassword,
                Picture = user.UserPicture // Depois o controller precisa receber IFormData e o arquivo como File e preciso subir em alguma CDN para ser consumido.
            });

            return newUser;
        }

        public async Task<User> RegisterUserByPlatform(User user)
        {
            User newUser = user;

            newUser.Id = await _dbConnection.ExecuteScalarAsync<Guid>(@"
            INSERT INTO USERS_PROFILE_DATA (user_name, user_email, user_password)
            VALUES (@Name, @Email, @Password)
            RETURNING id;",
            new
            {
                Name = user.UserName,
                Email = user.UserEmail,
                Password = user.UserPassword,
            });

            await InsertUserPlatformData(newUser.Id, (Platform)user.UserSignUpPlatform!, user.UserPlatformId!);

            return newUser;
        }

        public async Task InsertUserPlatformData(Guid userId, Platform platform, string platformId)
        {
            await _dbConnection.ExecuteScalarAsync<Guid>(@"
            INSERT INTO USERS_PROFILE_PLATFORM_DATA (user_id, user_signup_platform, user_platform_id)
            VALUES (@UserId, @Platform, @PlatformId);",
            new
            {
                UserId = userId,
                Platform = platform.ToString(),
                PlatformId = platformId,
            });
        }

        public async Task<bool> IsEmailBeingUsed(string email)
        {
            Guid userId = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(@"
            SELECT 
                  id
            FROM
                  USERS_PROFILE_DATA
            WHERE
                  user_email = @Email;",
            new
            {
                Email = email,
            });

            return userId == Guid.Empty;
        }
    }
}
