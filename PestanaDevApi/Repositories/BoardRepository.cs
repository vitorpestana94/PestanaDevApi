using System.Data;
using Dapper;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Factories;
using PestanaDevApi.Interfaces.Repositories;
using Sql = PestanaDevApi.Constants.Queries.BoardQueries;
using Params = PestanaDevApi.Utils.DapperParams;
using PestanaDevApi.Extensions.Dtos.Requests;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Models;

namespace PestanaDevApi.Repositories
{
    public class BoardRepository: IBoardRepository
    {
        private readonly IDbConnectionFactory _factory;

        public BoardRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task InsertBoard(Guid userId, CreateBoardRequestDto dto)
        {
            using IDbConnection db = _factory.CreateConnection();

            await db.ExecuteAsync(Sql.Insert, dto.ToInsert(userId));
        }


        public async Task<IEnumerable<Board>?> GetUserBoardsIds(Guid userId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryAsync<Board>(Sql.Select,Params.ToUserId(userId));
        }

        public async Task<(Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns)?> GetBoardData(Guid boardId)
        {
            using IDbConnection db = _factory.CreateConnection();

            using var result = await db.QueryMultipleAsync(Sql.SelectBoardData, Params.ToId(boardId));

            Board? board = await result.ReadFirstOrDefaultAsync<Board>();

            if (board == null)
                return null;

            IEnumerable<BoardColumn> columns = await result.ReadAsync<BoardColumn>();
            IEnumerable<BoardTask>? tasks = await result.ReadAsync<BoardTask>();
            IEnumerable<User> boardUsers = await result.ReadAsync<User>();

            return (Board: board, Tasks: tasks, BoardUsers: boardUsers, BoardColumns: columns);
        }
    }
}
