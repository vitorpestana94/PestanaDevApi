using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Interfaces.Services.Auth;
namespace PestanaDevApi.Services.Auth
{
    public class GitHubAuthService: IGitHubAuthService
    {
        private readonly IConfiguration _config;
        private readonly IRequestService _requestService;
        private readonly string _githubAppName;
        private readonly string _githubEndPoint;

        public GitHubAuthService(IConfiguration configuration, IRequestService requestService)
        {
            _config = configuration;
            _requestService = requestService;

            if (string.IsNullOrEmpty(_config["github.appname"]))
                throw new InvalidOperationException("Github App Name not configured!");

            if (string.IsNullOrEmpty(_config["github.endpoint"]))
                throw new InvalidOperationException("Github end-poin not configured!");

            _githubAppName = _config["github.appname"]!;
            _githubEndPoint = _config["github.endpoint"]!;
        }

        /// <summary>
        /// Validates the provided Github's access_token. If valid, it can return an existing user via Github's user email response 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the access_token is invalid.
        /// </summary>
        /// <param name="accessToken">The Github's acess_token.</param>
        /// <returns>Existing user data or create new data after registering.</returns>
        public async Task<(GithubResponseDto response, string userEmail)?> ValidateGitHubAcessToken(string accessToken)
        {
            Dictionary<string, string> headers = GetGitHubHeaders(accessToken);

            GithubResponseDto? response = await ValidateAccessToken(accessToken, headers);

            if (response == null)
                return null;

            string? userEmail = await HandleMissingGitHubEmail(response, accessToken, headers);

            if (userEmail == null)
                return null;

            return (response, userEmail);
        }

        /// <summary>
        /// Returns Github's request headers for acess_token validation
        /// </summary>
        /// <param name="accessToken">The Github acess_token to validate.</param>
        /// <returns>Github's request headers.</returns>
        private Dictionary<string, string> GetGitHubHeaders(string accessToken)
        {
            return new Dictionary<string, string>() { { "Authorization", $"Bearer {accessToken}" }, { "User-Agent", _githubAppName } };
        }

        /// <summary>
        /// Validates a GitHub's acess_token.
        /// </summary>
        /// <param name="accessToken">The Github acess_token to validate.</param>
        /// <param name="headers">The Github request headers</param>
        /// <returns>The Github's validation acess_token response.</returns>
        private async Task<GithubResponseDto?> ValidateAccessToken(string accessToken, Dictionary<string, string> headers)
        {
            try
            {
                return await _requestService.RequestAsync<GithubResponseDto>(_githubEndPoint, headers);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Fetchs Github to get all user emails.
        /// </summary>
        /// <param name="accessToken">The Github acess_token to validate.</param>
        /// <param name="headers">The Github request headers</param>
        /// <returns>The Github's validation acess_token response.</returns>
        private async Task<string?> GetGithubUserEmail(string accessToken, Dictionary<string, string> headers)
        {
            try
            {
                IEnumerable<GitHubResponseEmailDto?>? emailsResponse = await _requestService.RequestAsync<IEnumerable<GitHubResponseEmailDto>>($"{_githubEndPoint}/emails", headers);

                string email = emailsResponse?.FirstOrDefault(e => e != null && e.Primary && e.Verified)?.Email ?? string.Empty;

                if (string.IsNullOrEmpty(email))
                    return null;

                return email;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Handle the missing user email
        /// </summary>
        /// <param name="accessToken">The Github acess_token to validate.</param>
        /// <param name="headers">The Github request headers</param>
        /// <param name="response">The Github user response</param>
        /// <returns>User email on GitHub.</returns>
        private async Task<string?> HandleMissingGitHubEmail(GithubResponseDto response, string accessToken, Dictionary<string, string> headers)
        {
            return string.IsNullOrEmpty(response.Email) ? await GetGithubUserEmail(accessToken, headers) : response.Email;
        }
    }
}
