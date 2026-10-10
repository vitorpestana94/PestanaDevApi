using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class CreateBoardExtensionRequestDto
    {
        public static object ToInsert(this CreateBoardRequestDto dto, Guid userId) =>
        new
        {
            UserId = userId,
            dto.Name,
            dto.ColumnsNames.ColumnNameOne,
            dto.ColumnsNames.ColumnNameTwo,
            dto.ColumnsNames.ColumnNameThree,
            dto.ColumnsNames.ColumnNameFour,
            dto.ColumnsNames.ColumnNameFive,
            dto.BoardTask.TaskName,
            dto.BoardTask.TaskType,
            dto.BoardTask.TaskDescription
        };
    }
}
