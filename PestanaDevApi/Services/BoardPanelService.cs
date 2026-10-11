using System.Net;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Extensions.Dtos.Requests;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;

namespace PestanaDevApi.Services
{
    public class BoardPanelService: IBoardPanelService
    {
        private readonly IBoardPanelRepository _repository;
        private readonly IBoardMemberRepository _memberRepository;
        private readonly IBoardCacheService _cacheService;
        private readonly IBoardNotificationService _notificationService;

        public BoardPanelService(IBoardPanelRepository repository, IBoardMemberRepository memberRepository, IBoardCacheService cacheService, IBoardNotificationService notificationService)
        {
            _repository = repository;
            _memberRepository = memberRepository;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        public async Task<UpdateBoardResponseDto> UpdateBoard(Guid userId, Guid boardId, UpdateBoardRequestDto dto)
        {
            if (dto.WasDataNotUpdated())
                return new(ErrorMessage.BoardRequestDontHaveAnyChangedData);

            Board? board = await _repository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            if (!await _repository.UpdateBoard(boardId, dto))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.BoardNotUpdated);

            _cacheService.DeleteBoardDataCache(boardId);
            await DeleteMembersBoardsCache(boardId);

            Board? updatedBoard = await _repository.GetBoard(boardId);

            if (updatedBoard != null)
                await _notificationService.NotifyBoardUpdated(userId, updatedBoard);

            return new();
        }

        public async Task<DeleteBoardResponseDto> DeleteBoard(Guid userId, Guid boardId)
        {
            Board? board = await _repository.GetBoard(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.BoardNotFound);

            if (!board.IsBoardOwner(userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardOwner);

            // The members are read before the delete, since the delete also removes them.
            IEnumerable<User> members = await _memberRepository.GetBoardMembers(boardId);

            if (!await _repository.DeleteBoard(boardId))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.BoardNotDeleted);

            _cacheService.DeleteBoardDataCache(boardId);
            _cacheService.DeleteBoardMembersCache(boardId);
            DeleteMembersBoardsCache(members);

            await _notificationService.NotifyBoardDeleted(userId, boardId);

            return new();
        }

        #region Private Methods
        /// <summary>
        /// Removes the cached boards list of every board member, since the list shows the board name.
        /// </summary>
        private async Task DeleteMembersBoardsCache(Guid boardId) => DeleteMembersBoardsCache(await _memberRepository.GetBoardMembers(boardId));

        private void DeleteMembersBoardsCache(IEnumerable<User> members)
        {
            foreach (User member in members)
            {
                _cacheService.DeleteUserBoardsCache(member.Id);
            }
        }
        #endregion
    }
}
