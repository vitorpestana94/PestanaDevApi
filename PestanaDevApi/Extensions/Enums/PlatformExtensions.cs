using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Extensions.Enums
{
    public static class PlatformExtensions
    {
        public static object ToInsert(this PlatformEnum platform, Guid userId, string platformId) =>
        new
        {
            UserId = userId,
            Platform = platform.ToString(),
            PlatformId = platformId
        };
    }
}
