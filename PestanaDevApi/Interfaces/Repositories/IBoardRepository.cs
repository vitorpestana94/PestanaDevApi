using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IBoardRepository
    {
        Task InsertBoard(Guid userId, CreateBoardRequestDto dto);
    }
}
