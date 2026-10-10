using System.Net;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;


namespace PestanaDevApi.Services
{
    public class BoardService: IBoardService
    {
        private readonly IBoardRepository _repository;

        public BoardService(IBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateBoardResponseDto> CreateBoard(Guid userId, CreateBoardRequestDto dto)
        {

            await _repository.InsertBoard(userId, dto);

            return new();
        }

        public async Task<GetBoardsResponseDto>  GetUserBoardsIds(Guid userId)
        {
            IEnumerable<Board>? boards = await _repository.GetUserBoardsIds(userId);

            if (boards == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.NotFound);

            return new GetBoardsResponseDto(boards);
        }

        public async Task<GetBoardDataResponseDto> GetBoardData(Guid boardId)
        {
            (Board Board, IEnumerable<BoardTask>? Tasks, IEnumerable<User> BoardUsers, IEnumerable<BoardColumn> BoardColumns)? board = await _repository.GetBoardData(boardId);

            if (board == null)
                return new(HttpStatusCode.NotFound, ErrorMessage.NotFound);

            return new(board.Value);
        }

    }
}
