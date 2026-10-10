using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Extensions.Models
{
    public static class BoardExtensions
    {
        public static object ToBoardsIds(this IEnumerable<Board> boards) => new { Ids = boards.Select(board => board.Id) };
    }
}
