using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces
{
    public interface IBoardMemberService
    {
        /// <summary>
        /// Retrieves the members of a board. Only board members can see them.
        /// The response is cached until a member is added or removed.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// A response containing the board members (with the owner flagged) or an error response.
        /// </returns>
        Task<GetBoardMembersResponseDto> GetBoardMembers(Guid userId, Guid boardId);

        /// <summary>
        /// Adds a registered user to the board by email. Only the board owner can do it.
        /// Runs inside the board semaphore to avoid adding the same user twice. After the insert, the board cache
        /// is cleared and the board members are notified through SignalR (MemberAdded).
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="dto">
        /// Request containing the email of the user to be added.
        /// </param>
        /// <returns>
        /// A success response or an error response (board or user not found, forbidden, already a member or internal error).
        /// </returns>
        Task<AddBoardMemberResponseDto> AddBoardMember(Guid userId, Guid boardId, AddBoardMemberRequestDto dto);

        /// <summary>
        /// Removes a member from the board. The board owner can remove any other member (but can't be removed)
        /// and any member can leave the board. The tasks owned by the member are left without owner.
        /// Runs inside the board semaphore to avoid concurrent changes. After the delete, the board cache is cleared,
        /// the board members are notified through SignalR (MemberRemoved) and the member stops receiving the board events.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="memberId">
        /// Identifier of the member to be removed.
        /// </param>
        /// <returns>
        /// A success response or an error response (board or member not found, forbidden, owner can't be removed or internal error).
        /// </returns>
        Task<RemoveBoardMemberResponseDto> RemoveBoardMember(Guid userId, Guid boardId, Guid memberId);
    }
}
