using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Repositories;

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
    }
}
