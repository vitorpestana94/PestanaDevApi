using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IPlatformAuthService
    {
        Task<User?> HandleGoogleIdToken(string idToken);
        Task<User?> HandleGitHubAcessToken(string acessToken);
    }
}
