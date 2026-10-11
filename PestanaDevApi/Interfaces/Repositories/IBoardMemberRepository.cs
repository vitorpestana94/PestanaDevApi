using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IBoardMemberRepository
    {
        /// <summary>
        /// Retrieves the members of a board ordered by the date they joined it.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <returns>
        /// The collection of board members (id, name and email).
        /// </returns>
        Task<IEnumerable<User>> GetBoardMembers(Guid boardId);

        /// <summary>
        /// Retrieves a registered user by email.
        /// </summary>
        /// <param name="email">
        /// The user's email.
        /// </param>
        /// <returns>
        /// The user (id, name and email) if it exists; otherwise, <see langword="null"/>.
        /// </returns>
        Task<User?> GetUserByEmail(string email);

        /// <summary>
        /// Adds a user to the board members.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="userId">
        /// Identifier of the user to be added.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the member was added; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> InsertMember(Guid boardId, Guid userId);

        /// <summary>
        /// Removes a user from the board members, in a single transaction,
        /// leaving the tasks owned by the member without owner.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="userId">
        /// Identifier of the member to be removed.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the member was removed; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> DeleteMember(Guid boardId, Guid userId);

        /// <summary>
        /// Retrieves a registered user by id.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the user.
        /// </param>
        /// <returns>
        /// The user (id, name and email) if it exists; otherwise, <see langword="null"/>.
        /// </returns>
        Task<User?> GetUserById(Guid userId);

        /// <summary>
        /// Saves a pending invitation for a not registered email. If the email was already invited
        /// to the board, the invitation is only refreshed.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="email">
        /// The invited email.
        /// </param>
        /// <param name="invitedBy">
        /// Identifier of the user who sent the invitation (the board owner).
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the invitation was saved; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> InsertInvitation(Guid boardId, string email, Guid invitedBy);

        /// <summary>
        /// Checks if there's a pending invitation of the email to the board.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="email">
        /// The invited email.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the email was invited; otherwise, <see langword="false"/>.
        /// </returns>
        Task<bool> IsUserInvited(Guid boardId, string email);

        /// <summary>
        /// Deletes the pending invitation of the email to the board.
        /// </summary>
        /// <param name="boardId">
        /// Identifier of the board.
        /// </param>
        /// <param name="email">
        /// The invited email.
        /// </param>
        Task DeleteInvitation(Guid boardId, string email);
    }
}
