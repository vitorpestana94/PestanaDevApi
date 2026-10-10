using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardTaskService
    {
        /// <summary>
        /// Updates the data of a task and/or moves it to another column of the same board.
        /// Only board members can do it, and the new task owner (if sent) must also be a board member.
        /// Runs inside the board semaphore to avoid concurrent changes. After the update, the board cache
        /// is cleared and the board members are notified through SignalR (TaskUpdated).
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board that contains the task.
        /// </param>
        /// <param name="taskId">
        /// Identifier of the task to be updated.
        /// </param>
        /// <param name="dto">
        /// Request containing the optional new column, owner, name, type and description of the task.
        /// </param>
        /// <returns>
        /// A success response or an error response (no changes, forbidden, task or column not found, invalid owner or internal error).
        /// </returns>
        Task<UpdateBoardTaskResponseDto> UpdateTask(Guid userId, Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto);

        /// <summary>
        /// Deletes a task from a board. Only board members can do it.
        /// Runs inside the board semaphore to avoid concurrent changes. After the delete, the board cache
        /// is cleared and the board members are notified through SignalR (TaskDeleted).
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board that contains the task.
        /// </param>
        /// <param name="taskId">
        /// Identifier of the task to be deleted.
        /// </param>
        /// <returns>
        /// A success response or an error response (forbidden, not found or internal error).
        /// </returns>
        Task<DeleteBoardTaskResponseDto> DeleteTask(Guid userId, Guid boardId, Guid taskId);
    }
}
