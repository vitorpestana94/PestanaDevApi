namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IConfirmationCodeGenerationRepository
    {
        /// <summary>
        /// Inserts a confirmation code into the database associated with the user's email.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="code">A sequence of integers representing the confirmation code.</param>
        /// <returns>
        /// A task representing the asynchronous insert operation.
        /// </returns>
        /// <remarks>
        /// The code is persisted using a parameterized SQL query to prevent SQL injection.
        /// </remarks>
        Task InsertConfirmationCode(string email, string code);

        /// <summary>
        /// Checks whether there is at least one confirmation code record associated with the given email.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>
        /// <c>true</c> if a record exists for the specified email; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method executes a query that returns a constant value (1) if a matching record exists.
        /// It is intended to be used as an existence check rather than retrieving full entity data.
        /// </remarks>
        Task<bool> SelectOneIfTheresEmail(string email);

        /// <summary>
        /// Retrieves the confirmation code associated with the provided email.
        /// </summary>
        /// <param name="email">The email address used to look up the confirmation code.</param>
        /// <returns>
        /// A string containing the confirmation code if found; otherwise, an empty string.
        /// </returns>
        /// <remarks>
        /// Executes a query to fetch the confirmation code for the given email.
        /// If no record is found, returns an empty string instead of null.
        /// Relies on QueryFirstOrDefaultAsync to safely handle cases where no data exists.
        /// </remarks>
        Task<string> SelectCodeByEmail(string email);

        Task<bool> SelectOneIfCodeIsStillFresh(string email);

        Task DeleteConfirmationCode(string email);
    }
}
