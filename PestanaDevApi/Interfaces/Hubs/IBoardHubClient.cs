using PestanaDevApi.Dtos.Notifications;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Interfaces.Hubs
{
    public interface IBoardHubClient
    {
        Task BoardUpdated(BoardNotificationDto<Board> notification);
        Task BoardDeleted(BoardNotificationDto<Guid> notification);
        Task TaskCreated(BoardNotificationDto<BoardTask> notification);
        Task TaskUpdated(BoardNotificationDto<BoardTask> notification);
        Task TaskDeadlineChanged(BoardNotificationDto<TaskDeadlineChangedDto> notification);
        Task TaskDeleted(BoardNotificationDto<Guid> notification);
        Task ColumnsUpdated(BoardNotificationDto<IEnumerable<BoardColumn>> notification);
        Task MemberAdded(BoardNotificationDto<BoardMember> notification);
        Task MemberRemoved(BoardNotificationDto<Guid> notification);
    }
}
