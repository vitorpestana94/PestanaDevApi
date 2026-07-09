namespace PestanaDevApi.Utils
{
    public static class DapperParams
    {
        public static object ToUserEmail(string userEmail) => new { UserEmail = userEmail };

        public static object ToInsertRefreshToken(Guid userId, string deviceId, string token) =>
        new
        {
            Token = token,
            ExpiredAt = DateTime.UtcNow.AddDays(2),
            UserId = userId,
            DeviceId = deviceId
        };

        public static object ToEmail(string email) => new { Email = email };
        public static object ToUserId(Guid userId) => new { UserId = userId };
    }
}
