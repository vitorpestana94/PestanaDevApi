using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IBoardPanelRepository
    {
        /// <summary>
        /// Retrieves a board by its identifier.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// The board if it exists; otherwise, <see langword="null"/>.
        /// </returns>
        Task<Board?> GetBoard(Guid boardId);

        /// <summary>
        /// Checks if the user is a member of the board.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="userId">
        /// Identifier of the user.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the user is a board member; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> IsBoardMember(Guid boardId, Guid userId);

        /// <summary>
        /// Updates the name and/or the description of a board. Null fields keep their current value.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="dto">
        /// Request containing the optional new name and description.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the board was updated; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> UpdateBoard(Guid boardId, UpdateBoardRequestDto dto);

        /// <summary>
        /// Deletes, in a single transaction, the board tasks, columns, members and the board itself.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the board was deleted; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> DeleteBoard(Guid boardId);
    }
}
