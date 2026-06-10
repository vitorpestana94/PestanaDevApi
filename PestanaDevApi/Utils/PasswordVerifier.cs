namespace PestanaDevApi.Utils
{
    public static class PasswordVerifier
    {
        /// <summary>
        /// Get a user profile properties if exists.
        /// <param name="dtoPassword">The provided password on request.</param>
        /// <param name="userPassword">The stored password hash.</param>
        /// <returns>True if the provided password matchs with the stored hash, false otherwise.</returns>
        /// </summary>
        public static bool IsPasswordNotValid(string dtoPassword, string userPassword)
        {
            return string.IsNullOrEmpty(userPassword) || !BCrypt.Net.BCrypt.Verify(dtoPassword, userPassword);
        }
    }
}
