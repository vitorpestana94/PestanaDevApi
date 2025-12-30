using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;

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
            INSERT INTO USERS_PROFILE_DATA (user_name, user_email, user_password, user_picture, user_signup_platform)
            VALUES (@Name, @Email, @Password, @Picture, @Platform)
            RETURNING id;",
            new
            {
                Name = user.UserName,
                Email = user.UserEmail,
                Password = user.UserPassword,
                Picture = user.UserPicture,
                Platform = user.UserSignUpPlatform.ToString()
            });

            return newUser;
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
