using System.Data;
using Dapper;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.BoardQueries;
using Params = PestanaDevApi.Utils.DapperParams;
using PestanaDevApi.Models;

namespace PestanaDevApi.Repositories
{
    public class BoardMemberRepository: IBoardMemberRepository
    {
        private readonly IDbConnectionFactory _factory;

        public BoardMemberRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<User>> GetBoardMembers(Guid boardId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryAsync<User>(Sql.SelectBoardMembers, Params.ToBaordId(boardId));
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<User?>(Sql.SelectUserByEmail, Params.ToEmail(email));
        }

        public async Task<bool> InsertMember(Guid boardId, Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.InsertMember, Params.ToBoardIdAndUserId(boardId, userId)) > 0;
        }

        public async Task<bool> DeleteMember(Guid boardId, Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.DeleteMember, Params.ToBoardIdAndUserId(boardId, userId)) > 0;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<User?>(Sql.SelectUserById, Params.ToUserId(userId));
        }

        #region Invitations
        public async Task<bool> InsertInvitation(Guid boardId, string email, Guid invitedBy)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.InsertInvitation, Params.ToInsertInvitation(boardId, email, invitedBy)) > 0;
        }

        public async Task<bool> IsUserInvited(Guid boardId, string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteScalarAsync<bool>(Sql.SelectIsUserInvited, Params.ToBoardIdAndEmail(boardId, email));
        }

        public async Task DeleteInvitation(Guid boardId, string email)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.DeleteInvitation, Params.ToBoardIdAndEmail(boardId, email));
        }
        #endregion
    }
}
