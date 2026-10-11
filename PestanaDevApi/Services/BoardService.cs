using System.Net;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;

namespace PestanaDevApi.Services
{
    public class BoardService: IBoardService
    {
        private readonly IBoardRepository _repository;
        private readonly IBoardCacheService _cacheService;

        public BoardService(IBoardRepository repository, IBoardCacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<CreateBoardResponseDto> CreateBoard(Guid userId, CreateBoardRequestDto dto)
        {
            await _repository.InsertBoard(userId, dto);

            _cacheService.DeleteUserBoardsCache(userId);

            return new();
        }

        public async Task<GetBoardsResponseDto> GetUserBoardsIds(Guid userId)
        {
            return _cacheService.GetUserBoardsCache(userId) ?? await GetUserBoardsIdsFromDB(userId);
        }

        public async Task<GetBoardDataResponseDto> GetBoardData(Guid boardId)
        {
            return _cacheService.GetBoardDataCache(boardId) ?? await GettBoardDataFromDB(boardId);
        }

        #region Private Methods
        /// <summary>
        /// Retrieves the boards owned by the user from the database and caches the response.
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
