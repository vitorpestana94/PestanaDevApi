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
    }
}
