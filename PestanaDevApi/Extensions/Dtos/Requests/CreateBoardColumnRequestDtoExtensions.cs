using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class CreateBoardColumnRequestDtoExtensions
    {
        public static object ToInsert(this CreateBoardColumnRequestDto dto, Guid boardId) =>
        new
        {
            ColumnId = Guid.NewGuid(),
            BoardId = boardId,
            dto.Name
        };
    }
}
