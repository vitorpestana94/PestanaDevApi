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

        #region Board
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
        #endregion

        #region Tasks
        public async Task<BoardTask?> GetTask(Guid boardId, Guid taskId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<BoardTask?>(Sql.SelectTask, Params.ToBoardIdAndTaskId(boardId, taskId));
        }

        public async Task<bool> UpdateTask(Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.UpdateTask, dto.ToUpdate(boardId, taskId)) > 0;
        }

        public async Task<bool> DeleteTask(Guid boardId, Guid taskId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.DeleteTask, Params.ToBoardIdAndTaskId(boardId, taskId)) > 0;
        }
        #endregion

        #region Columns
        public async Task<IEnumerable<BoardColumn>> GetBoardColumns(Guid boardId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryAsync<BoardColumn>(Sql.SelectBoardColumns, Params.ToBaordId(boardId));
        }

        public async Task<BoardColumn?> InsertColumn(Guid boardId, CreateBoardColumnRequestDto dto)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.QueryFirstOrDefaultAsync<BoardColumn?>(Sql.InsertColumn, dto.ToInsert(boardId));
        }

        public async Task<bool> UpdateColumn(UpdateBoardColumnRequestDto dto, BoardColumn currentColumn, int columnsCount)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.UpdateColumn, dto.ToUpdate(currentColumn, columnsCount)) > 0;
        }

        public async Task<bool> DeleteColumn(Guid boardId, Guid columnId)
        {
            using IDbConnection db = _factory.CreateConnection();

            return await db.ExecuteAsync(Sql.DeleteColumn, Params.ToBoardIdAndColumnId(boardId, columnId)) > 0;
        }
        #endregion

        #region Members
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
        #endregion
    }
}
