using System.Net;
using PestanaDevApi.Constants;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Extensions.Dtos.Requests;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;

namespace PestanaDevApi.Services
{
    public class BoardColumnService: IBoardColumnService
    {
        private readonly IBoardColumnRepository _repository;
        private readonly IBoardPanelRepository _boardRepository;
        private readonly ISemaphoreService _semaphoreService;
        private readonly IBoardCacheService _cacheService;
        private readonly IBoardNotificationService _notificationService;

        public BoardColumnService(IBoardColumnRepository repository, IBoardPanelRepository boardRepository, [FromKeyedServices(SemaphoreConstants.BoardSemaphoreKey)] ISemaphoreService semaphoreService, IBoardCacheService cacheService, IBoardNotificationService notificationService)
        {
            _repository = repository;
            _boardRepository = boardRepository;
            _semaphoreService = semaphoreService;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        public async Task<UpdateBoardColumnResponseDto> UpdateColumn(Guid userId, Guid boardId, Guid columnId, UpdateBoardColumnRequestDto dto)
        {
            return await _semaphoreService.Work(() => UpdateBoardColumn(userId, boardId, columnId, dto));
        }

        #region Private Methods
        /// <summary>
        /// Validates and renames and/or reorders a column.
        /// Runs inside the board semaphore because the current position is read before the update:
        /// without the lock, two concurrent reorders would use a stale position and break the columns order.
        /// </summary>
        private async Task<UpdateBoardColumnResponseDto> UpdateBoardColumn(Guid userId, Guid boardId, Guid columnId, UpdateBoardColumnRequestDto dto)
        {
            if (dto.WasDataNotUpdated())
                return new(ErrorMessage.ColumnRequestDontHaveAnyChangedData);

            if (!await _boardRepository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            IEnumerable<BoardColumn> columns = await _repository.GetBoardColumns(boardId);
            BoardColumn? column = columns.GetColumn(columnId);

            if (column == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.ColumnNotFound);

            if (dto.ColumnNameWasUpdated(currentColumnName: column.Name) && columns.HasColumnName(dto.Name!))
                return new(ErrorMessage.ColumnNameAlreadyExists);

            if (!await _repository.UpdateColumn(dto, column, columns.Count()))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.ColumnNotUpdated);

            _cacheService.DeleteBoardDataCache(boardId);

            IEnumerable<BoardColumn> updatedColumns = await _repository.GetBoardColumns(boardId);

            await _notificationService.NotifyColumnsUpdated(userId, boardId, updatedColumns);

            return new();
        }
        #endregion
    }
}
