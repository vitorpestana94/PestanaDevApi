using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;

namespace PestanaDevApi.Repositories
{
    public class SignUpRepository: GenericRepository, ISignUpRepository
    {
        public readonly IDbConnection _dbConnection;

        public SignUpRepository(IDbConnection dbConnection):base(dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> InsertUser(User user)
        {
            return await _dbConnection.ExecuteScalarAsync<Guid>(@"
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
        }
    }
}
