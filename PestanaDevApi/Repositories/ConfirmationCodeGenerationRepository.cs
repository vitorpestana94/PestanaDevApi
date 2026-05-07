using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Factories;
using Sql = PestanaDevApi.Constants.Queries.ConfirmationCodeQueries;
using Params = PestanaDevApi.Utils.DapperParams;

namespace PestanaDevApi.Repositories
{
    public class ConfirmationCodeGenerationRepository:  IConfirmationCodeGenerationRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ConfirmationCodeGenerationRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task InsertConfirmationCode(string email, string code)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.InserConfirmationCode, new { UserEmail = email, ConfirmationCode = code });
        }

        public async Task UpdateConfirmationCode(string email, string code)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.UpdateConfirmationCode, new { UserEmail = email, ConfirmationCode = code });
        }

        public async Task<bool> SelectOneIfTheresEmail(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<bool>(Sql.SelectOneIfTheresEmail, Params.ToUserEmail(email));
        }

        public async Task<string> SelectCodeByEmail(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<string>(Sql.SelectCodeByEmail, Params.ToUserEmail(email)) ?? string.Empty;
        }

        public async Task<bool> SelectOneIfCodeIsStillFresh(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<bool>(Sql.SelectOneIfCodeIsStillFresh, Params.ToUserEmail(email));
        }

        public async Task DeleteConfirmationCode(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.DeleteConfirmationCode, Params.ToUserEmail(email));
        }

        public async Task DeleteUnfreshConfirmationCodes()
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.DeleteUnfreshConfirmationCodes);
        }

        public async Task<bool> CheckCreatedAt(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            int rowsAffected = await db.ExecuteAsync(Sql.UpdateConfirmationCodeCreatedAtIfValid, Params.ToUserEmail(email));

            return rowsAffected > 0;
        }
    }
}
