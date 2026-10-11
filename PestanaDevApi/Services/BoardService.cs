using System.Net;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;
using PestanaDevApi.Extensions.Dtos.Requests;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;

namespace PestanaDevApi.Services
{
    public class BoardService: IBoardService
    {
        private readonly IBoardRepository _repository;
        private readonly IBoardPanelRepository _boardPanelRepository;
        private readonly IBoardCacheService _cacheService;

        public BoardService(IBoardRepository repository, IBoardPanelRepository boardPanelRepository, IBoardCacheService cacheService)
        {
            _repository = repository;
            _boardPanelRepository = boardPanelRepository;
            _cacheService = cacheService;
        }

        public async Task<CreateBoardResponseDto> CreateBoard(Guid userId, CreateBoardRequestDto dto)
        {
            // Without this check the database unique constraint (board_id, name) would return an internal error.
            if (dto.ColumnsNames.HasDuplicatedNames())
                return new(HttpStatusCode.BadRequest, ErrorMessage.BoardColumnsNamesMustBeUnique);

            await _repository.InsertBoard(userId, dto);

            _cacheService.DeleteUserBoardsCache(userId);

            return new();
        }

        public async Task<GetBoardsResponseDto> GetUserBoardsIds(Guid userId)
        {
            return _cacheService.GetUserBoardsCache(userId) ?? await GetUserBoardsIdsFromDB(userId);
        }

        public async Task<GetBoardDataResponseDto> GetBoardData(Guid userId, Guid boardId)
        {
            if (!await _boardPanelRepository.IsBoardMember(boardId, userId))
                return new(HttpStatusCode.Forbidden, ErrorMessage.UserIsNotBoardMember);

            return _cacheService.GetBoardDataCache(boardId) ?? await GettBoardDataFromDB(boardId);
        }

        #region Private Methods
        /// <summary>
        /// Retrieves the boards where the user is a member from the database and caches the response.
        /// </summary>
        private async Task<GetBoardsResponseDto> GetUserBoardsIdsFromDB(Guid userId)
        {
            IEnumerable<Board>? boards = await _repository.GetUserBoardsIds(userId);

            if (boards == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.NotFound);

            GetBoardsResponseDto response = new(boards);

            _cacheService.SetUserBoardsCache(userId, response);

            return response;
        }

        /// <summary>
        /// Retrieves the board data from the database and caches the response.
        /// </summary>
        private async Task<GetBoardDataResponseDto> GettBoardDataFromDB(Guid boardId)
        {
            (Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns)? board = await _repository.GetBoardData(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.NotFound);

            GetBoardDataResponseDto response = new(board.Value);

            _cacheService.SetBoardDataCache(boardId, response);

            return response;
        }
        #endregion
    }
}
