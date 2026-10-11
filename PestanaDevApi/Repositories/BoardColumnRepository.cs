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
    public class BoardColumnRepository: IBoardColumnRepository
    {
        private readonly IDbConnectionFactory _factory;

        public BoardColumnRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<BoardColumn>> GetBoardColumns(Guid boardId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryAsync<BoardColumn>(Sql.SelectBoardColumns, Params.ToBaordId(boardId));
        }

        public async Task<bool> UpdateColumn(UpdateBoardColumnRequestDto dto, BoardColumn currentColumn, int columnsCount)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.UpdateColumn, dto.ToUpdate(currentColumn, columnsCount)) > 0;
        }
    }
}
