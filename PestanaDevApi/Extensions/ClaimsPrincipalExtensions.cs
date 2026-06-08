using System.Security.Claims;

namespace PestanaDevApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal cp) => Guid.Parse(cp.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
