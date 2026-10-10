using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IBoardCacheService
    {
        /// <summary>
        /// Retrieves the cached board data (board, columns, tasks and users).
        /// </summary>
        /// <param name="boardId">
        /// The board identifier.
        /// </param>
        /// <returns>
        /// The cached board data if available; otherwise, <see langword="null"/>.
        /// </returns>
        GetBoardDataResponseDto? GetBoardDataCache(Guid boardId);

        /// <summary>
        /// Stores the board data in the cache.
        /// </summary>
        /// <param name="boardId">
        /// The board identifier.
        /// </param>
        /// <param name="response">
        /// The response to be cached.
        /// </param>
        void SetBoardDataCache(Guid boardId, GetBoardDataResponseDto response);

        /// <summary>
        /// Retrieves the cached list of boards owned by the user.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The cached boards if available; otherwise, <see langword="null"/>.
        /// </returns>
        GetBoardsResponseDto? GetUserBoardsCache(Guid userId);

        /// <summary>
        /// Stores the list of boards owned by the user in the cache.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <param name="response">
        /// The response to be cached.
        /// </param>
        void SetUserBoardsCache(Guid userId, GetBoardsResponseDto response);

        /// <summary>
        /// Retrieves the cached list of board members.
        /// </summary>
        /// <param name="boardId">
        /// The board identifier.
        /// </param>
        /// <returns>
        /// The cached members if available; otherwise, <see langword="null"/>.
        /// </returns>
        GetBoardMembersResponseDto? GetBoardMembersCache(Guid boardId);

        /// <summary>
        /// Stores the list of board members in the cache.
        /// </summary>
        /// <param name="boardId">
        /// The board identifier.
        /// </param>
        /// <param name="response">
        /// The response to be cached.
        /// </param>
        void SetBoardMembersCache(Guid boardId, GetBoardMembersResponseDto response);

        /// <summary>
        /// Removes the cached board data. Must be called after any change on the board.
        /// </summary>
        /// <param name="boardId">
        /// The board identifier.
        /// </param>
        void DeleteBoardDataCache(Guid boardId);

        /// <summary>
        /// Removes the cached list of boards owned by the user.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        void DeleteUserBoardsCache(Guid userId);

        /// <summary>
        /// Removes the cached list of board members.
        /// </summary>
        /// <param name="boardId">
        /// The board identifier.
        /// </param>
        void DeleteBoardMembersCache(Guid boardId);
    }
}
