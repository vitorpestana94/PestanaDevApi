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
        /// Adds a user to the board by email:
        /// - the owner adds a registered user: the user is added right away;
        /// - the owner adds a not registered email: a pending invitation is saved and an email (in the request locale)
        ///   invites the user to sign up, with a button to the client side sign up page (?redirectTo=scrum-board&amp;boardId={boardId});
        /// - a non owner sends their own email: the user joins only if they have a pending invitation (called by the front end after the sign up).
        /// Runs inside the board semaphore to avoid adding the same user twice (the email is sent after it). After the insert, the board cache
        /// is cleared and the board members are notified through SignalR (MemberAdded).
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user making the request.
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="dto">
        /// Request containing the email of the user to be added (or invited) and the locale of the invitation email.
        /// </param>
        /// <returns>
        /// A success response (with InvitationSent = true when an invitation was sent) or an error response
        /// (invalid email, board not found, forbidden, not invited, already a member or internal error).
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
