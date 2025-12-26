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

        /// <summary>
        /// Returns the error status code via the thrown exception.
        /// <param name="error">The thrown error</param>
        /// <returns>
        /// Error status code
        /// </returns>
        /// </summary>
        public static int GetErrorStatusCode(Exception? error)
        {
            return error switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status403Forbidden,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        /// <summary>
        /// Returns the error message by the thrown error and it status code.
        /// <param name="error">The thrown error</param>
        /// <param name="statusCode">The thrown error status code</param>
        /// <returns>
        /// Error message
        /// </returns>
        /// </summary>
        public static string GetErrorMessage(int statusCode, Exception? error)
        {
            return statusCode == 500 ? "Internal Server Error!" : error?.Message ?? "error!";
        }
    }
}
