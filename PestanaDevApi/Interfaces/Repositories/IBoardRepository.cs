using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IBoardRepository
    {
        /// <summary>
        /// Inserts, in a single transaction, a new board, its owner as the first member,
        /// the 5 default columns and the first task on the first column.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user who will own the board.
        /// </param>
        /// <param name="dto">
        /// Request containing the board name, the 5 columns names and the first task data.
        /// </param>
        Task InsertBoard(Guid userId, CreateBoardRequestDto dto);

        /// <summary>
        /// Retrieves the boards owned by the user.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user who owns the boards.
        /// </param>
        /// <returns>
        /// The collection of boards owned by the user.
        /// </returns>
        Task<IEnumerable<Board>?> GetUserBoardsIds(Guid userId);

        /// <summary>
        /// Retrieves, in a single database round trip, the board, its columns, tasks and members.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// A tuple containing the board data, or <see langword="null"/> if the board doesn't exist.
        /// </returns>
        Task<(Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns)?> GetBoardData(Guid boardId);
    }
}
