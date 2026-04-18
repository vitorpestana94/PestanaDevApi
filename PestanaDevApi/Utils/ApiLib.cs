using System.Net;
using System.Text.RegularExpressions;
using ErrorMessage = PestanaDevApi.Constants.ErrorMessages;

namespace PestanaDevApi.Utils
{
    public static class ApiLib
    {
        /// <summary>
        /// Validates whether a given string is in a correct email format.
        /// <param name="email">The email address to validate.</param>
        /// <returns>
        /// Returns the email is valid, otherwise and error will be thrown.
        /// </returns>
        /// <exception cref="BadRequestException">Thrown if the provided email is empty somehow or if the format does not correspond with the correct format.</exception>
        /// </summary>
        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

            return emailRegex.IsMatch(email);
        }

        public static string GenerateRandomCode(int codeLenght = 8)
        {
            return Guid.NewGuid().ToString().Replace("-", "")[..codeLenght];
        }
    }
}
