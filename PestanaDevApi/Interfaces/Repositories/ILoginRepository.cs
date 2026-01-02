using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ILoginRepository
    {
        Task<User?> GetUserDataByEmail(string email);
        Task<Guid> GetUserIdByPlatformId(Platform platform, string platformId);
        Task<Guid> GetUserIdEmail(string email);
    }
}
