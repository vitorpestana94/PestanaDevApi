using System.Data;
using Dapper;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.BoardQueries;
using Params = PestanaDevApi.Utils.DapperParams;
using PestanaDevApi.Extensions.Dtos.Requests;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Repositories
{
    public class BoardPanelRepository: IBoardPanelRepository
    {
        private readonly IDbConnectionFactory _factory;

        public BoardPanelRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<Board?> GetBoard(Guid boardId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<Board?>(Sql.SelectBoardById, Params.ToBaordId(boardId));
        }

        public async Task<bool> IsBoardMember(Guid boardId, Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteScalarAsync<bool>(Sql.SelectIsBoardMember, Params.ToBoardIdAndUserId(boardId, userId));
        }

        public async Task<bool> UpdateBoard(Guid boardId, UpdateBoardRequestDto dto)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.UpdateBoard, dto.ToUpdate(boardId)) > 0;
        }

        public async Task<bool> DeleteBoard(Guid boardId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.DeleteBoard, Params.ToBaordId(boardId)) > 0;
        }
    }
}
