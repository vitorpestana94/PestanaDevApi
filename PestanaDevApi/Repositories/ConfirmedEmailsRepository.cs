using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.ConfirmedEmailsQueries;
using Params = PestanaDevApi.Utils.DapperParams;

namespace PestanaDevApi.Repositories
{
    public class ConfirmedEmailsRepository: IConfirmedEmailsRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ConfirmedEmailsRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task InsertCofirmedEmail(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.Insert, Params.ToUserEmail(email));
        }

        public async Task<bool> IsEmailConfirmed(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<bool>(Sql.SelectExistEmail, Params.ToUserEmail(email));
        }
    }
}
