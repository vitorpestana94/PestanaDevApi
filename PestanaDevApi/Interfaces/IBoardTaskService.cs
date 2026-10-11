using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardTaskService
    {
        /// <summary>
        /// Creates a task on a column of the board. Only board members can do it, and the task owner (if sent)
        /// must also be a board member; when it's not sent, the user creating the task becomes its owner. The deadline is optional.
        /// Runs inside the board semaphore to avoid concurrent changes. After the insert, the board cache
        /// is cleared and the board members are notified through SignalR (TaskCreated).
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="dto">
        /// Request containing the column, the optional owner, name, type and description of the task.
        /// </param>
        /// <returns>
        /// A response containing the created task or an error response (forbidden, column not found, invalid owner or internal error).
        /// </returns>
        Task<CreateBoardTaskResponseDto> CreateTask(Guid userId, Guid boardId, CreateBoardTaskRequestDto dto);

        /// <summary>
        /// Updates the data of a task and/or moves it to another column of the same board.
        /// Only board members can do it, and the new task owner (if sent) must also be a board member.
        /// The deadline can be set/changed (Deadline) or removed (ClearDeadline = true).
        /// Runs inside the board semaphore to avoid concurrent changes. After the update, the board cache
        /// is cleared and the board members are notified through SignalR (TaskUpdated, plus TaskDeadlineChanged
        /// when the deadline changed).
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
