using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Interfaces.Services.Auth
{
    public interface IPlatformAuthService
    {
        Task<User?> GetUserByIToken(string token, Platform platform);
    }
}
