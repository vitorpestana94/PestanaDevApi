using System.Globalization;
using System.Text.RegularExpressions;

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

        public static string GenerateRandomCode(int codeLenght = 4)
        {
            return Guid.NewGuid().ToString().Replace("-", "")[..codeLenght];
        }

        public static bool IsInvalidSecondsGap(string issuedAt, int seconds = 30)
        {
            if (string.IsNullOrEmpty(issuedAt))
                return true;

            DateTime iat = DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedAt)).UtcDateTime;

            return (DateTime.UtcNow - iat) <= TimeSpan.FromSeconds(seconds);
        }

        public static bool CheckDate(string date) => DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d);
    }
}
