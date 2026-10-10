using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IBoardRepository
    {
        Task InsertBoard(Guid userId, CreateBoardRequestDto dto);
        Task<IEnumerable<Board>?> GetUserBoardsIds(Guid userId);
        Task<(Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns)?> GetBoardData(Guid boardId);
        Task<Board?> GetBoard(Guid boardId);
        Task<bool> IsBoardMember(Guid boardId, Guid userId);
        Task<bool> UpdateBoard(Guid boardId, UpdateBoardRequestDto dto);
        Task<bool> DeleteBoard(Guid boardId);
        Task<BoardTask?> GetTask(Guid boardId, Guid taskId);
        Task<bool> UpdateTask(Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto);
        Task<bool> DeleteTask(Guid boardId, Guid taskId);
        Task<IEnumerable<BoardColumn>> GetBoardColumns(Guid boardId);
        Task<BoardColumn?> InsertColumn(Guid boardId, CreateBoardColumnRequestDto dto);
        Task<bool> UpdateColumn(UpdateBoardColumnRequestDto dto, BoardColumn currentColumn, int columnsCount);
        Task<bool> DeleteColumn(Guid boardId, Guid columnId);
        Task<IEnumerable<User>> GetBoardMembers(Guid boardId);
        Task<User?> GetUserByEmail(string email);
        Task<bool> InsertMember(Guid boardId, Guid userId);
        Task<bool> DeleteMember(Guid boardId, Guid userId);
    }
}
