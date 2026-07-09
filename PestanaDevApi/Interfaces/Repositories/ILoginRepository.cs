using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ILoginRepository
    {
        Task<User?> GetUserDataByEmail(string email);
        Task<Guid> GetUserIdByPlatformId(PlatformEnum platform, string platformId);
        Task<Guid> GetUserIdByEmail(string email);
        Task DeleteRefreshToken(Guid userId, string deviceId);
    }
}
