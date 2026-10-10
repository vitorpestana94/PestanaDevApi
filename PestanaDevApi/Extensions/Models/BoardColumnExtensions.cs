using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Extensions.Models
{
    public static class BoardColumnExtensions
    {
        public static bool HasColumnName(this IEnumerable<BoardColumn> columns, string name) =>
            columns.Any(column => string.Equals(column.Name, name, StringComparison.OrdinalIgnoreCase));

        public static BoardColumn? GetColumn(this IEnumerable<BoardColumn> columns, Guid columnId) =>
            columns.FirstOrDefault(column => column.Id == columnId);
    }
}
