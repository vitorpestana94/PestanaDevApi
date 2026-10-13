using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PestanaDevApi.Constants;
using PestanaDevApi.Extensions;
using PestanaDevApi.Interfaces.Hubs;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;
using Consts = PestanaDevApi.Constants.SignalRConstants;

namespace PestanaDevApi.Hubs
{
    /// <summary>
    /// Real time channel of the boards. The client connects, calls JoinBoard(boardId)
    /// and starts receiving the events declared on <see cref="IBoardHubClient"/>.
    /// Only accepts hub tokens (POST /auth/hub-token), never the main JWT.
    /// </summary>
    [Authorize(AuthenticationSchemes = TokenConstants.HubTokenScheme)]
    public class BoardHub: Hub<IBoardHubClient>
    {
        private readonly IBoardPanelRepository _boardRepository;
        private readonly IBoardHubConnectionService _connectionService;
        private Guid UserId => Context.User!.GetUserId();

        public BoardHub(IBoardPanelRepository boardRepository, IBoardHubConnectionService connectionService)
        {
            _boardRepository = boardRepository;
            _connectionService = connectionService;
        }

        public static string GetGroupName(Guid boardId) => $"{Consts.BoardGroupPrefix}{boardId}";

        public override async Task OnConnectedAsync()
        {
            _connectionService.AddConnection(UserId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _connectionService.RemoveConnection(UserId, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Starts listening to the changes of a board. Only board members can join.
        /// </summary>
        public async Task JoinBoard(Guid boardId)
        {
            if (!await _boardRepository.IsBoardMember(boardId, UserId))
                throw new HubException(ErrorMessage.UserIsNotBoardMember);

            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(boardId));
        }

        /// <summary>
        /// Stops listening to the changes of a board.
        /// </summary>
        public async Task LeaveBoard(Guid boardId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(boardId));
        }
    }
}
