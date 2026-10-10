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
    public class BoardTaskService: IBoardTaskService
    {
        private readonly IBoardTaskRepository _repository;
        private readonly IBoardPanelRepository _boardRepository;
        private readonly IBoardColumnRepository _columnRepository;
        private readonly ISemaphoreService _semaphoreService;
        private readonly IBoardCacheService _cacheService;
        private readonly IBoardNotificationService _notificationService;

        public BoardTaskService(
            IBoardTaskRepository repository,
            IBoardPanelRepository boardRepository,
            IBoardColumnRepository columnRepository,
            [FromKeyedServices(SemaphoreConstants.BoardSemaphoreKey)] ISemaphoreService semaphoreService,
            IBoardCacheService cacheService,
            IBoardNotificationService notificationService)
        {
            _repository = repository;
            _boardRepository = boardRepository;
            _columnRepository = columnRepository;
            _semaphoreService = semaphoreService;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        public async Task<UpdateBoardTaskResponseDto> UpdateTask(Guid userId, Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto)
        {
            return await _semaphoreService.Work(() => UpdateBoardTask(userId, boardId, taskId, dto));
        }

        public async Task<DeleteBoardTaskResponseDto> DeleteTask(Guid userId, Guid boardId, Guid taskId)
        {
            return await _semaphoreService.Work(() => DeleteBoardTask(userId, boardId, taskId));
        }

        #region Private Methods
        /// <summary>
        /// Validates and updates (or moves) a task.
        /// Runs inside the board semaphore so the validations and the update can't be
        /// interleaved with another request changing the same data.
        /// </summary>
        private async Task<UpdateBoardTaskResponseDto> UpdateBoardTask(Guid userId, Guid boardId, Guid taskId, UpdateBoardTaskRequestDto dto)
        {
            if (dto.WasDataNotUpdated())
                return new(ErrorMessage.TaskRequestDontHaveAnyChangedData);

            if (!await _boardRepository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            BoardTask? task = await _repository.GetTask(boardId, taskId);

            if (task == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.TaskNotFound);

            if (dto.IsMovingTask())
            {
                IEnumerable<BoardColumn> columns = await _columnRepository.GetBoardColumns(boardId);

                if (columns.GetColumn(dto.ColumnId!.Value) == null)
                    return new(HttpStatusCode.NotFound, ErrorMessage.ColumnNotFound);
            }

            if (dto.IsChangingTaskOwner() && !await _boardRepository.IsBoardMember(boardId, dto.TaskOwnerId!.Value))
                return new(ErrorMessage.TaskOwnerIsNotBoardMember);

            if (!await _repository.UpdateTask(boardId, taskId, dto))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.TaskNotUpdated);

            _cacheService.DeleteBoardDataCache(boardId);

            BoardTask? updatedTask = await _repository.GetTask(boardId, taskId);

            if (updatedTask != null)
                await _notificationService.NotifyTaskUpdated(userId, updatedTask);

            return new();
        }

        /// <summary>
        /// Validates and deletes a task.
        /// Runs inside the board semaphore so a task can't be deleted while another request is updating it.
        /// </summary>
        private async Task<DeleteBoardTaskResponseDto> DeleteBoardTask(Guid userId, Guid boardId, Guid taskId)
        {
            if (!await _boardRepository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            BoardTask? task = await _repository.GetTask(boardId, taskId);

            if (task == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.TaskNotFound);

            if (!await _repository.DeleteTask(boardId, taskId))
                return new(HttpStatusCode.InternalServerError, ErrorMessage.TaskNotDeleted);

            _cacheService.DeleteBoardDataCache(boardId);

            await _notificationService.NotifyTaskDeleted(userId, boardId, taskId);

            return new();
        }
        #endregion
    }
}
