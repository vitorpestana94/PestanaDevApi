using System.Security.Claims;

namespace PestanaDevApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal cp) => Guid.Parse(cp.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        public static string GetDeviceId(this ClaimsPrincipal cp) => Guid.Parse(cp.FindFirst("sid")!.Value).ToString();
    }
}
