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

        public BoardMemberService(IBoardMemberRepository repository,IBoardPanelRepository boardRepository,[FromKeyedServices(SemaphoreConstants.BoardSemaphoreKey)] ISemaphoreService semaphoreService,IBoardCacheService cacheService,IBoardNotificationService notificationService)
        {
            _repository = repository;
            _boardRepository = boardRepository;
            _semaphoreService = semaphoreService;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        public async Task<GetBoardMembersResponseDto> GetBoardMembers(Guid userId, Guid boardId)
        {
            if (!await _boardRepository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            return _cacheService.GetBoardMembersCache(boardId) ?? await RequestBoardMembers(boardId);
        }

        public async Task<AddBoardMemberResponseDto> AddBoardMember(Guid userId, Guid boardId, AddBoardMemberRequestDto dto)
        {
            return await _semaphoreService.Work(() => InsertBoardMember(userId, boardId, dto));
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
        /// Validates and adds a user to the board.
        /// Runs inside the board semaphore so two concurrent requests can't both pass the
        /// "is already a member" check and try to insert the same user.
        /// </summary>
        private async Task<AddBoardMemberResponseDto> InsertBoardMember(Guid userId, Guid boardId, AddBoardMemberRequestDto dto)
        {
            Board? board = await _boardRepository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            User? user = await _repository.GetUserByEmail(dto.Email);

            if (user == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.UserNotFound);

            if (await _boardRepository.IsBoardMember(boardId, user.Id))
                return new(ErrorMessage.UserIsAlreadyBoardMember);

            if (!await _repository.InsertMember(boardId, user.Id))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.MemberNotAdded);

            _cacheService.DeleteBoardDataCache(boardId);
            _cacheService.DeleteBoardMembersCache(boardId);

            await _notificationService.NotifyMemberAdded(userId, boardId, new BoardMember(user, isOwner: false));

            return new();
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

            await _notificationService.NotifyMemberRemoved(userId, boardId, memberId);

            return new();
        }
        #endregion
    }
}
