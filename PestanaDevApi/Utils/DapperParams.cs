namespace PestanaDevApi.Utils
{
    public static class DapperParams
    {
        public static object ToUserEmail(string userEmail) => new { UserEmail = userEmail };
    }
}
