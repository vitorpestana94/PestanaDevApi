using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Interfaces.Services
{
    /// <summary>
    /// Reports the board changes to the board members connected through SignalR.
    /// Every method receives "changedBy" so the client can ignore its own changes.
    /// </summary>
    public interface IBoardNotificationService
    {
        Task NotifyBoardUpdated(Guid changedBy, Board board);
        Task NotifyBoardDeleted(Guid changedBy, Guid boardId);
        Task NotifyTaskCreated(Guid changedBy, BoardTask task);
        Task NotifyTaskUpdated(Guid changedBy, BoardTask task);

        /// <summary>
        /// Notifies the board members that the deadline of a task was set, changed or removed.
        /// </summary>
        Task NotifyTaskDeadlineChanged(Guid changedBy, BoardTask task, DateTime? oldDeadline);
        Task NotifyTaskDeleted(Guid changedBy, Guid boardId, Guid taskId);
        Task NotifyColumnsUpdated(Guid changedBy, Guid boardId, IEnumerable<BoardColumn> columns);
        Task NotifyMemberAdded(Guid changedBy, Guid boardId, BoardMember member);

        /// <summary>
        /// Notifies the board and removes the member's connections from the board group,
        /// so the removed member stops receiving the board changes.
        /// </summary>
        Task NotifyMemberRemoved(Guid changedBy, Guid boardId, Guid memberId);
    }
}
