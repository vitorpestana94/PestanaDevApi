using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class UpdateBoardRequestDtoExtensions
    {
        public static object ToUpdate(this UpdateBoardRequestDto dto, Guid boardId) =>
        new
        {
            BoardId = boardId,
            Name = string.IsNullOrEmpty(dto.Name) ? null : dto.Name,
            dto.Description
        };

        public static bool WasDataNotUpdated(this UpdateBoardRequestDto dto) => string.IsNullOrEmpty(dto.Name) && dto.Description == null;
    }
}
