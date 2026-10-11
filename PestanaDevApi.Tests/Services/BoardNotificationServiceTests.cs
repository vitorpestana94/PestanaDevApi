using Microsoft.AspNetCore.SignalR;
using Moq;
using PestanaDevApi.Dtos.Notifications;
using PestanaDevApi.Hubs;
using PestanaDevApi.Interfaces.Hubs;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Services;

namespace PestanaDevApi.Tests.Services
{
    public class BoardNotificationServiceTests
    {
        private readonly Mock<IHubContext<BoardHub, IBoardHubClient>> _hubContext = new();
        private readonly Mock<IBoardHubClient> _boardGroup = new();
        private readonly Mock<IGroupManager> _groups = new();
        private readonly BoardHubConnectionService _connections = new();
        private readonly BoardNotificationService _service;
        private readonly Guid _boardId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        public BoardNotificationServiceTests()
        {
            Mock<IHubClients<IBoardHubClient>> clients = new();
            clients.Setup(c => c.Group(BoardHub.GetGroupName(_boardId))).Returns(_boardGroup.Object);
            _hubContext.Setup(h => h.Clients).Returns(clients.Object);
            _hubContext.Setup(h => h.Groups).Returns(_groups.Object);
            _service = new(_hubContext.Object, _connections);
        }

        [Fact]
        public async Task NotifyTaskUpdated_SendsTheTaskToTheBoardGroup()
        {
            BoardTask task = new() { Id = Guid.NewGuid(), BoardId = _boardId, TaskName = "Task" };

            await _service.NotifyTaskUpdated(_userId, task);

            _boardGroup.Verify(g => g.TaskUpdated(It.Is<BoardNotificationDto<BoardTask>>(n => n.BoardId == _boardId && n.ChangedBy == _userId && n.Data == task)), Times.Once);
        }

        [Fact]
        public async Task NotifyTaskDeadlineChanged_SendsOldAndNewDeadlineToTheBoardGroup()
        {
            DateTime oldDeadline = new(2026, 10, 20);
            BoardTask task = new() { Id = Guid.NewGuid(), BoardId = _boardId, TaskName = "Task", Deadline = null };

            await _service.NotifyTaskDeadlineChanged(_userId, task, oldDeadline);

            _boardGroup.Verify(g => g.TaskDeadlineChanged(It.Is<BoardNotificationDto<TaskDeadlineChangedDto>>(n =>
                n.Data.TaskId == task.Id && n.Data.OldDeadline == oldDeadline && n.Data.NewDeadline == null && n.ChangedBy == _userId)), Times.Once);
        }

        [Fact]
        public async Task NotifyMemberRemoved_NotifiesAndRemovesTheMemberConnectionsFromTheGroup()
        {
            Guid memberId = Guid.NewGuid();
            _connections.AddConnection(memberId, "connection-1");

            await _service.NotifyMemberRemoved(_userId, _boardId, memberId);

            _boardGroup.Verify(g => g.MemberRemoved(It.Is<BoardNotificationDto<Guid>>(n => n.Data == memberId)), Times.Once);
            _groups.Verify(g => g.RemoveFromGroupAsync("connection-1", BoardHub.GetGroupName(_boardId), default), Times.Once);
        }
    }
}
