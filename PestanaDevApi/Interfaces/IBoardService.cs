using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardService
    {
        /// <summary>
        /// Creates a new board owned by the user, together with its 5 default columns
        /// and a first task placed on the first column. The 5 columns names must be different from each other.
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
        /// Retrieves the ids and names of the boards where the user is a member (owned or shared with the user).
        /// The response is cached until one of these boards is created, updated or deleted, or the user joins or leaves a board.
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
        /// Only board members can see it. The response is cached until something changes on the board.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// A response containing the board data or an error response (forbidden or not found).
        /// </returns>
        Task<GetBoardDataResponseDto> GetBoardData(Guid userId, Guid boardId);
    }
}
