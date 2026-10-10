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
    public class BoardTaskRepository: IBoardTaskRepository
    {
        private readonly IDbConnectionFactory _factory;

        public BoardTaskRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

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
    }
}
