using Google.Apis.Auth;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Services
{
    public class PlatformAuthService : IPlatformAuthService
    {
        private readonly IConfiguration _config;
        private readonly ILoginRepository _loginRepository;
        private readonly ISignUpRepository _signUpRepository;
        private readonly IRequestService _requestService;
        private readonly string _googleClientId;
        private readonly string _githubAppName;
        private readonly string _githubEndPoint;


        public PlatformAuthService(IConfiguration configuration, ILoginRepository loginRepository, ISignUpRepository signUpRepository, IRequestService requestService) 
        {
            _config = configuration;
            _loginRepository = loginRepository;
            _signUpRepository = signUpRepository;
            _requestService = requestService;

            if (string.IsNullOrEmpty(_config["google.clientid.web"]))
                throw new InvalidOperationException("Google client id not configured!");

            if (string.IsNullOrEmpty(_config["github.appname"]))
                throw new InvalidOperationException("Github App Name not configured!");

            if (string.IsNullOrEmpty(_config["github.endpoint"]))
                throw new InvalidOperationException("Github end-poin not configured!");

            _googleClientId = _config["google.clientid.web"]!;
            _githubAppName = _config["github.appname"]!;
            _githubEndPoint = _config["github.endpoint"]!;
        }

        /// <summary>
        /// Validates the provided idToken.If valid, it can return an existing user via Google's payload email 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the provided ID token is invalid.
        /// </summary>
        /// <param name="idToken">The Google's ID toke.</param>
        /// <returns>Existing user data or create new data after registering.</returns>
        public async Task<User?> HandleGoogleIdToken(string idToken)
        {
            GoogleJsonWebSignature.Payload? response = await ValidateGoogleToken(idToken);

            if (response == null)
                return null;

            Guid userId = await GetUserIdByPlatformOrEmail(Platform.Google, response.Subject, response.Email);

            return userId != Guid.Empty ? new User(response, userId) : await _signUpRepository.RegisterUserByPlatform(new User(response));
        }

        /// <summary>
        /// Validates the provided Github's access_token. If valid, it can return an existing user via Github's user email response 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the access_token is invalid.
        /// </summary>
        /// <param name="acessToken">The Github's acess_token.</param>
        /// <returns>Existing user data or create new data after registering.</returns>
        public async Task<User?> HandleGitHubAcessToken(string accessToken)
        {
            Dictionary<string, string> headers = new() { { "Authorization", $"Bearer {accessToken}" }, { "User-Agent", _githubAppName } };
            GithubResponseDto? response = await ValidateGitHubAccessToken(accessToken, headers);

            if (response == null)
                return null;
           
            string? userEmail = await HandleMissingGitHubEmail(response, accessToken, headers);

            if (userEmail == null)
                return null;

            Guid userId = await GetUserIdByPlatformOrEmail(Platform.GitHub, response.Id.ToString(), userEmail);

            return userId != Guid.Empty ? new User(response, userId, userEmail) : await _signUpRepository.RegisterUserByPlatform(new User(response, userEmail));
        }

        /// <summary>
        /// Returns the user id by platform and platformid or from userEmail
        /// </summary>
        /// <param name="platform">Auth platform</param>
        /// <param name="platformId">Auth platform id</param>
        /// <param name="userEmail">User email returned by platform auth.</param>
        /// <returns>User Id on database or Guid.Empty.</returns>
        private async Task<Guid> GetUserIdByPlatformOrEmail(Platform platform,string platformId, string userEmail)
        {
            Guid userId = await _loginRepository.GetUserIdByPlatformId(platform, platformId);

            if (userId == Guid.Empty)
            {
                userId = await _loginRepository.GetUserIdEmail(userEmail);

                if(userId != Guid.Empty)
                    await _signUpRepository.InsertUserPlatformData(userId, platform, platformId);
            }

            return userId;
        }

        /// <summary>
        /// Validates a Google  and ensures it matches configured client IDs.
        /// </summary>
        /// <param name="idToken">The Google ID token to validate.</param>
        /// <returns>The payload of the validated Google ID token.</returns>
        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken)
        {
            try
            {
             return await GoogleJsonWebSignature.ValidateAsync(idToken,
             new GoogleJsonWebSignature.ValidationSettings
             {
                 Audience = [_googleClientId]
             });
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Validates a GitHub's acess_token.
        /// </summary>
        /// <param name="accessToken">The Github acess_token to validate.</param>
        /// <param name="headers">The Github request headers</param>
        /// <returns>The Github's validation acess_token response.</returns>
        private async Task<GithubResponseDto?> ValidateGitHubAccessToken(string accessToken, Dictionary<string, string> headers)
        {
            try
            {
                return await _requestService.GetAsync<GithubResponseDto>(_githubEndPoint, headers);
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
                IEnumerable<GitHubResponseEmailDto?>? emailsResponse = await _requestService.GetAsync<IEnumerable<GitHubResponseEmailDto>>($"{_githubEndPoint}/emails", headers);

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
