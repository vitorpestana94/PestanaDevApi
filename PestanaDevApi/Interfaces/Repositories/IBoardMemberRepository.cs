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
    }
}
