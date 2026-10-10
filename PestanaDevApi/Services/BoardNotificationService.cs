using Microsoft.AspNetCore.SignalR;
using PestanaDevApi.Dtos.Notifications;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Hubs;
using PestanaDevApi.Interfaces.Hubs;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Services
{
    public class BoardNotificationService: IBoardNotificationService
    {
        private readonly IHubContext<BoardHub, IBoardHubClient> _hubContext;
        private readonly IBoardHubConnectionService _connectionService;

        public BoardNotificationService(IHubContext<BoardHub, IBoardHubClient> hubContext, IBoardHubConnectionService connectionService)
        {
            _hubContext = hubContext;
            _connectionService = connectionService;
        }

        #region Board
        public async Task NotifyBoardUpdated(Guid changedBy, Board board)
        {
            await GetBoardGroup(board.Id).BoardUpdated(new(board.Id, changedBy, board));
        }

        public async Task NotifyBoardDeleted(Guid changedBy, Guid boardId)
        {
            await GetBoardGroup(boardId).BoardDeleted(new(boardId, changedBy, boardId));
        }
        #endregion

        #region Tasks
        public async Task NotifyTaskUpdated(Guid changedBy, BoardTask task)
        {
            await GetBoardGroup(task.BoardId).TaskUpdated(new(task.BoardId, changedBy, task));
        }

        public async Task NotifyTaskDeleted(Guid changedBy, Guid boardId, Guid taskId)
        {
            await GetBoardGroup(boardId).TaskDeleted(new(boardId, changedBy, taskId));
        }
        #endregion

        #region Columns
        public async Task NotifyColumnsUpdated(Guid changedBy, Guid boardId, IEnumerable<BoardColumn> columns)
        {
            await GetBoardGroup(boardId).ColumnsUpdated(new(boardId, changedBy, columns));
        }
        #endregion

        #region Members
        public async Task NotifyMemberAdded(Guid changedBy, Guid boardId, BoardMember member)
        {
            await GetBoardGroup(boardId).MemberAdded(new(boardId, changedBy, member));
        }

        public async Task NotifyMemberRemoved(Guid changedBy, Guid boardId, Guid memberId)
        {
            await GetBoardGroup(boardId).MemberRemoved(new(boardId, changedBy, memberId));

            foreach (string connectionId in _connectionService.GetConnections(memberId))
            {
                await _hubContext.Groups.RemoveFromGroupAsync(connectionId, BoardHub.GetGroupName(boardId));
            }
        }
        #endregion

        #region Private Methods
        private IBoardHubClient GetBoardGroup(Guid boardId) => _hubContext.Clients.Group(BoardHub.GetGroupName(boardId));
        #endregion
    }
}
