using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.ForgotPasswordQueries;

namespace PestanaDevApi.Repositories
{
    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ForgotPasswordRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task ChangeUserPassword(string email, string password)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.UpdateUserPassword, new { Email = email, Password = password });
        }
    }
}
