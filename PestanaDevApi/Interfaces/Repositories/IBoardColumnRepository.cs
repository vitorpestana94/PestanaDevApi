using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IBoardColumnRepository
    {
        /// <summary>
        /// Retrieves the columns of a board ordered by position.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// The collection of the board columns ordered by position.
        /// </returns>
        Task<IEnumerable<BoardColumn>> GetBoardColumns(Guid boardId);

        /// <summary>
        /// Renames and/or moves a column to a new position, shifting the columns in between in a single transaction.
        /// See BoardQueries.UpdateColumn for the details of the reorder.
        /// </summary>
        /// <param name="dto">
        /// Request containing the optional new name and position of the column.
        /// </param>
        /// <param name="currentColumn">
        /// The column as it is now on the database (used to get its current position).
        /// </param>
        /// <param name="columnsCount">
        /// Amount of columns of the board (used to clamp the new position).
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the column was updated; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> UpdateColumn(UpdateBoardColumnRequestDto dto, BoardColumn currentColumn, int columnsCount);
    }
}
