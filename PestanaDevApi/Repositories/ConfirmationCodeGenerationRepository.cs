using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.ConfirmationCodeQueries;
using Params = PestanaDevApi.Utils.DapperParams;

namespace PestanaDevApi.Repositories
{
    public class ConfirmationCodeGenerationRepository: IConfirmationCodeGenerationRepository
    {
        private readonly IDbConnection _db;

        public ConfirmationCodeGenerationRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task InsertConfirmationCode(string email, string code)
        {
            await _db.ExecuteAsync(Sql.InserConfirmationCode, new { UserEmail = email, ConfirmationCode = code });
        }

        public async Task<bool> SelectOneIfTheresEmail(string email)
        {
            return await _db.QueryFirstOrDefaultAsync<bool>(Sql.SelectOneIfTheresEmail, Params.ToUserEmail(email));
        }

        public async Task<string> SelectCodeByEmail(string email)
        {
            return await _db.QueryFirstOrDefaultAsync<string>(Sql.SelectCodeByEmail, Params.ToUserEmail(email)) ?? string.Empty;
        }

        public async Task<bool> SelectOneIfCodeIsStillFresh(string email)
        {
            return await _db.QueryFirstOrDefaultAsync<bool>(Sql.SelectOneIfCodeIsStillFresh, Params.ToUserEmail(email));
        }

        public async Task DeleteConfirmationCode(string email)
        {
            await _db.ExecuteAsync(Sql.DeleteConfirmationCode, Params.ToUserEmail(email));
        }
    }
}
