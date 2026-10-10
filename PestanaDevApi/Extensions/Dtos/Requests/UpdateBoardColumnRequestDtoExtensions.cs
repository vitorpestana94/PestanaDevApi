using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class UpdateBoardColumnRequestDtoExtensions
    {
        public static object ToUpdate(this UpdateBoardColumnRequestDto dto, BoardColumn currentColumn, int columnsCount) =>
        new
        {
            BoardId = currentColumn.BoardId,
            ColumnId = currentColumn.Id,
            Name = string.IsNullOrEmpty(dto.Name) ? null : dto.Name,
            OldPosition = currentColumn.Position,
            NewPosition = dto.Position == null ? currentColumn.Position : Math.Clamp(dto.Position.Value, 1, columnsCount)
        };

        public static bool WasDataNotUpdated(this UpdateBoardColumnRequestDto dto) => string.IsNullOrEmpty(dto.Name) && dto.Position == null;

        public static bool ColumnNameWasUpdated(this UpdateBoardColumnRequestDto dto, string currentColumnName) =>
            !string.IsNullOrEmpty(dto.Name) && !string.Equals(dto.Name, currentColumnName, StringComparison.OrdinalIgnoreCase);
    }
}
