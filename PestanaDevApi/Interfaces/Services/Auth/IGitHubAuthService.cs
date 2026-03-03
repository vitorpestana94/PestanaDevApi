using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services.Auth
{
    public interface IGitHubAuthService
    {
        Task<(GithubResponseDto response, string userEmail)?> ValidateGitHubAcessToken(string accessToken);
    }
}
