using System.Net;
using PestanaDevApi.Constants;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Utils;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;

namespace PestanaDevApi.Services
{
    public class BoardMemberService: IBoardMemberService
    {
        private readonly IBoardMemberRepository _repository;
        private readonly IBoardPanelRepository _boardRepository;
        private readonly ISemaphoreService _semaphoreService;
        private readonly IBoardCacheService _cacheService;
        private readonly IBoardNotificationService _notificationService;
        private readonly IEmailService _emailService;

        public BoardMemberService(IBoardMemberRepository repository,IBoardPanelRepository boardRepository,[FromKeyedServices(SemaphoreConstants.BoardSemaphoreKey)] ISemaphoreService semaphoreService,IBoardCacheService cacheService,IBoardNotificationService notificationService, IEmailService emailService)
        {
            _repository = repository;
            _boardRepository = boardRepository;
            _semaphoreService = semaphoreService;
            _cacheService = cacheService;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<GetBoardMembersResponseDto> GetBoardMembers(Guid userId, Guid boardId)
        {
            if (!await _boardRepository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            return _cacheService.GetBoardMembersCache(boardId) ?? await RequestBoardMembers(boardId);
        }

        public async Task<AddBoardMemberResponseDto> AddBoardMember(Guid userId, Guid boardId, AddBoardMemberRequestDto dto)
        {
            (AddBoardMemberResponseDto response, BoardInvitationEmailDto? invitation) = await _semaphoreService.Work(() => InsertBoardMember(userId, boardId, dto));

            if (invitation != null)
                await _emailService.SendBoardInvitationEmail(invitation);

            return response;
        }

        public async Task<RemoveBoardMemberResponseDto> RemoveBoardMember(Guid userId, Guid boardId, Guid memberId)
        {
            return await _semaphoreService.Work(() => DeleteBoardMember(userId, boardId, memberId));
        }

        #region Private Methods
        /// <summary>
        /// Retrieves the board members from the database and caches the response.
        /// </summary>
        private async Task<GetBoardMembersResponseDto> RequestBoardMembers(Guid boardId)
        {
            Board? board = await _boardRepository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            IEnumerable<User> members = await _repository.GetBoardMembers(boardId);

            GetBoardMembersResponseDto response = new(members, board);

            _cacheService.SetBoardMembersCache(boardId, response);

            return response;
        }

        /// <summary>
        /// Validates the request and decides how the user joins the board:
        /// - the owner adds a registered user: the user is added right away;
        /// - the owner adds a not registered email: an invitation is saved and its email data is returned to be sent;
        /// - a non owner adds their own email: the user joins only if there's a pending invitation (sign up from the invitation email).
        /// Runs inside the board semaphore so two concurrent requests can't both pass the
        /// "is already a member" check and try to insert the same user.
        /// </summary>
        private async Task<(AddBoardMemberResponseDto Response, BoardInvitationEmailDto? Invitation)> InsertBoardMember(Guid userId, Guid boardId, AddBoardMemberRequestDto dto)
        {
            if (!ApiLib.IsEmailValid(dto.Email))
                return (new(ErrorMessage.InvalidEmailFormat), null);

            Board? board = await _boardRepository.GetBoard(boardId);

            if (board == null)
                return (new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound), null);

            User? user = await _repository.GetUserByEmail(dto.Email);

            if (!board.IsBoardOwner(userId))
                return (await AcceptInvitation(userId, boardId, user), null);

            if (user == null)
                return await InviteUser(userId, boardId, dto);

            return (await AddMember(userId, boardId, user), null);
        }

        /// <summary>
        /// Adds a registered user to the board, deletes any pending invitation of the user and notifies the board members.
        /// </summary>
        private async Task<AddBoardMemberResponseDto> AddMember(Guid changedBy, Guid boardId, User user)
        {
            if (await _boardRepository.IsBoardMember(boardId, user.Id))
                return new(ErrorMessage.UserIsAlreadyBoardMember);

            if (!await _repository.InsertMember(boardId, user.Id))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.MemberNotAdded);

            await _repository.DeleteInvitation(boardId, user.UserEmail);

            _cacheService.DeleteBoardDataCache(boardId);
            _cacheService.DeleteBoardMembersCache(boardId);
            _cacheService.DeleteUserBoardsCache(user.Id);

            await _notificationService.NotifyMemberAdded(changedBy, boardId, new BoardMember(user, isOwner: false));

            return new();
        }

        /// <summary>
        /// Lets a user who signed up from the invitation email join the board.
        /// The user can only accept their own invitation.
        /// </summary>
        private async Task<AddBoardMemberResponseDto> AcceptInvitation(Guid userId, Guid boardId, User? user)
        {
            if (user == null || user.Id != userId || !await _repository.IsUserInvited(boardId, user.UserEmail))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserWasNotInvitedToBoard);

            return await AddMember(userId, boardId, user);
        }

        /// <summary>
        /// Saves a pending invitation for a not registered email and returns the data of the invitation email.
        /// </summary>
        private async Task<(AddBoardMemberResponseDto Response, BoardInvitationEmailDto? Invitation)> InviteUser(Guid userId, Guid boardId, AddBoardMemberRequestDto dto)
        {
            User? owner = await _repository.GetUserById(userId);

            if (owner == null || !await _repository.InsertInvitation(boardId, dto.Email, userId))
                return (new(HttpStatusCode.InternalServerError, ErrorMessage.MemberNotInvited), null);

            return (new(invitationSent: true), new BoardInvitationEmailDto(boardId, dto.Email, owner.UserEmail, dto.ClientLocale));
        }

        /// <summary>
        /// Validates and removes a member from the board.
        /// Runs inside the board semaphore so the member can't be assigned to a task
        /// while their tasks are being unassigned.
        /// </summary>
        private async Task<RemoveBoardMemberResponseDto> DeleteBoardMember(Guid userId, Guid boardId, Guid memberId)
        {
            Board? board = await _boardRepository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId) && userId != memberId)
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            if (board.IsBoardOwner(memberId))
                return new(ErrorMessage.BoardOwnerCannotBeRemoved);

            if (!await _boardRepository.IsBoardMember(boardId, memberId))
                return new(HttpStatusCode.NotFound, ErrorMessage.MemberNotFound);

            if (!await _repository.DeleteMember(boardId, memberId))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.MemberNotRemoved);

            _cacheService.DeleteBoardDataCache(boardId);
            _cacheService.DeleteBoardMembersCache(boardId);
            _cacheService.DeleteUserBoardsCache(memberId);

            await _notificationService.NotifyMemberRemoved(userId, boardId, memberId);

            return new();
        }
        #endregion
    }
}
