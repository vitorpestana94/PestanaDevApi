using System.Security.Cryptography;
using System.Text;

namespace PestanaDevApi.Utils
{
    public static class SHA526Factory
    {
        public static string GenerateHmac(string code, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] codeBytes = Encoding.UTF8.GetBytes(code);

            using HMACSHA256 sha256 = new(keyBytes);
            byte[] hashBytes = sha256.ComputeHash(codeBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool ValidateCode(string inputCode, string storedHash, string secretKey)
        {
            string inputHash = GenerateHmac(inputCode, secretKey);

            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(inputHash), Convert.FromBase64String(storedHash));
        }
    }
}
