using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardService
    {
        /// <summary>
        /// Creates a new board owned by the user, together with its 5 default columns
        /// and a first task placed on the first column.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user who will own the board.
        /// </param>
        /// <param name="dto">
        /// Request containing the board name, the 5 columns names and the first task data.
        /// </param>
        /// <returns>
        /// A success response or an error response.
        /// </returns>
        Task<CreateBoardResponseDto> CreateBoard(Guid userId, CreateBoardRequestDto dto);

        /// <summary>
        /// Retrieves the ids and names of the boards owned by the user.
        /// The response is cached until a board of the user is created, updated or deleted.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user who owns the boards.
        /// </param>
        /// <returns>
        /// A response containing the user's boards or an error response.
        /// </returns>
        Task<GetBoardsResponseDto> GetUserBoardsIds(Guid userId);

        /// <summary>
        /// Retrieves all the data of a board: the board itself, its columns, tasks and members.
        /// The response is cached until something changes on the board.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// A response containing the board data or a not found response.
        /// </returns>
        Task<GetBoardDataResponseDto> GetBoardData(Guid boardId);
    }
}
