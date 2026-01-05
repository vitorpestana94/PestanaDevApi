using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services.Auth
{
    public interface IPlatformAuthService
    {
        Task<User?> HandleGoogleIdToken(string idToken);
        Task<User?> HandleGitHubAcessToken(string acessToken);
        Task<User?> HandleLinkedinIdToken(string idToken);
    }
}
