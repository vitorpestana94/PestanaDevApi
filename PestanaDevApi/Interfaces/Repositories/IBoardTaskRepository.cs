using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IBoardTaskRepository
    {
        /// <summary>
        /// Retrieves a task of a board.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board that contains the task.
        /// </param>
        /// <param name="taskId">
        /// Identifier of the task.
        /// </param>
        /// <returns>
        /// The task if it exists on the board; otherwise, <see langword="null"/>.
        /// </returns>
        Task<BoardTask?> GetTask(Guid boardId, Guid taskId);

        /// <summary>
        /// Updates the data and/or the column of a task. Null fields keep their current value.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board that contains the task.
        /// </param>
        /// <param name="taskId">
        /// Identifier of the task.
        /// </param>
        /// <param name="dto">
        /// Request containing the optional new column, owner, name, type and description of the task.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the task was updated; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> UpdateTask(Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto);

        /// <summary>
        /// Deletes a task of a board.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board that contains the task.
        /// </param>
        /// <param name="taskId">
        /// Identifier of the task.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the task was deleted; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> DeleteTask(Guid boardId, Guid taskId);
    }
}
