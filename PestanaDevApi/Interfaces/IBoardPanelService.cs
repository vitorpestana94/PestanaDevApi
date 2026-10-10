using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardPanelService
    {
        /// <summary>
        /// Updates the name and/or the description of a board. Only the board owner can do it.
        /// After the update, the board cache is cleared and the board members are notified through SignalR (BoardUpdated).
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board to be updated.
        /// </param>
        /// <param name="dto">
        /// Request containing the optional new name and description.
        /// </param>
        /// <returns>
        /// A success response or an error response (no changes, not found, forbidden or internal error).
        /// </returns>
        Task<UpdateBoardResponseDto> UpdateBoard(Guid userId, Guid boardId, UpdateBoardRequestDto dto);

        /// <summary>
        /// Deletes a board with all its columns, tasks and members. Only the board owner can do it.
        /// After the delete, the board cache is cleared and the board members are notified through SignalR (BoardDeleted).
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board to be deleted.
        /// </param>
        /// <returns>
        /// A success response or an error response (not found, forbidden or internal error).
        /// </returns>
        Task<DeleteBoardResponseDto> DeleteBoard(Guid userId, Guid boardId);
    }
}
