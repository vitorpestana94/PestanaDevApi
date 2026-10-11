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
            dto.BoardTask.TaskDescription,
            dto.BoardTask.Deadline
        };

        public static bool HasDuplicatedNames(this CreateColumnNames columnsNames)
        {
            string[] names = [columnsNames.ColumnNameOne, columnsNames.ColumnNameTwo, columnsNames.ColumnNameThree, columnsNames.ColumnNameFour, columnsNames.ColumnNameFive];

            return names.Distinct(StringComparer.OrdinalIgnoreCase).Count() != names.Length;
        }
    }
}
