using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ITokenService
    {
        Task<ApiToken> GenerateApiTokens(User user, string deviceId);
    }
}
