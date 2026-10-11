using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardColumnService
    {
        /// <summary>
        /// Renames and/or moves a column to a new position, shifting the columns in between.
        /// Only board members can do it, and the new name can't be used by another column of the board.
        /// A position out of range is clamped to the first or last position.
        /// Runs inside the board semaphore to avoid concurrent reorders. After the update, the board cache
        /// is cleared and the board members are notified through SignalR (ColumnsUpdated) with all the columns.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board that contains the column.
        /// </param>
        /// <param name="columnId">
        /// Identifier of the column to be updated.
        /// </param>
        /// <param name="dto">
        /// Request containing the optional new name and position of the column.
        /// </param>
        /// <returns>
        /// A success response or an error response (no changes, forbidden, not found, name already used or internal error).
        /// </returns>
        Task<UpdateBoardColumnResponseDto> UpdateColumn(Guid userId, Guid boardId, Guid columnId, UpdateBoardColumnRequestDto dto);
    }
}
