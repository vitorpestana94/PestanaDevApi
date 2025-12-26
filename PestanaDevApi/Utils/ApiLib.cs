using System.Text.RegularExpressions;
using PestanaDevApi.Exceptions;

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
        public static string IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new BadRequestException();

            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

            if(!emailRegex.IsMatch(email))
                throw new BadRequestException();

            return email;
        }
    }
}
