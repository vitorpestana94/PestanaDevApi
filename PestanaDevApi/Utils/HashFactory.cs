namespace PestanaDevApi.Utils
{
    public class HashFactory
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
