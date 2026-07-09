using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Interfaces.Services.Auth
{
    public interface IPlatformAuthService
    {
        Task<User?> GetUserByIoken(string token, PlatformEnum platform);
    }
}
