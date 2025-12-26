using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ITokenService
    {
        Task<ApiToken> GenerateApiTokens(Guid userId, string deviceId);
    }
}
