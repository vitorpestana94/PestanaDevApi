using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardService
    {
        Task<CreateBoardResponseDto> CreateBoard(Guid userId, CreateBoardRequestDto dto);
    }
}
