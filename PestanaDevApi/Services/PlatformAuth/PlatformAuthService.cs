using Google.Apis.Auth;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Interfaces.Services.Auth;
using System.IdentityModel.Tokens.Jwt;
using PestanaDevApi.Extensions;

namespace PestanaDevApi.Services.Auth
{
    public class PlatformAuthService : IPlatformAuthService
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IGitHubAuthService _gitHubAuthService;
        private readonly ILinkedinAuthService _linkedinAuthService;
        private readonly ILoginRepository _loginRepository;
        private readonly ISignUpRepository _signUpRepository;

        public PlatformAuthService(ILoginRepository loginRepository, ILinkedinAuthService linkedinAuthService, IGitHubAuthService gitHubAuthService, ISignUpRepository signUpRepository, IGoogleAuthService googleAuthService)
        {
            _loginRepository = loginRepository;
            _signUpRepository = signUpRepository;
            _gitHubAuthService = gitHubAuthService;
            _googleAuthService = googleAuthService;
            _linkedinAuthService = linkedinAuthService;
        }

        public async Task<User?> GetUserByIoken(string token, PlatformEnum platform)
        {
            return platform switch
            {
                PlatformEnum.Google => await HandleGoogleIdToken(token),
                PlatformEnum.GitHub => await HandleGitHubAcessToken(token),
                PlatformEnum.Linkedin => await HandleLinkedinIdToken(token),
                _ => null
            };
        }

        #region Private Methods
        /// <summary>
        /// Validates the provided idToken.If valid, it can return an existing user via Google's payload email 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the provided ID token is invalid.
        /// </summary>
        /// <param name="idToken">The Google's ID toke.</param>
        /// <returns>Existing user data or create new data after registering.</returns>
        private async Task<User?> HandleGoogleIdToken(string idToken)
        {
            GoogleJsonWebSignature.Payload? response = await _googleAuthService.ValidateGoogleToken(idToken);

            if (response == null)
                return null;

            Guid userId = await GetUserIdByPlatformOrEmail(PlatformEnum.Google, response.Subject, response.Email);

            return userId.IsNotEmpty() ? User.FromGoogleIdentity(response, userId) : await RegisterNewUser(new User(response));
        }

        /// <summary>
        /// Validates the provided Github's access_token. If valid, it can return an existing user via Github's user email response 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the access_token is invalid.
        /// </summary>
        /// <param name="accessToken">The Github's acess_token.</param>
        /// <returns>Existing user data or create new data after registering.</returns>
        private async Task<User?> HandleGitHubAcessToken(string accessToken)
        {
            (GithubResponseDto gitHubResponse, string userEmail)? response = await _gitHubAuthService.ValidateGitHubAcessToken(accessToken);

            if (response == null)
                return null;

            (GithubResponseDto gitHubResponse, string userEmail) = response.Value;

            Guid userId = await GetUserIdByPlatformOrEmail(PlatformEnum.GitHub, gitHubResponse.Id.ToString(), userEmail);

            return userId.IsNotEmpty() ? User.FromGitHubIdentity(gitHubResponse, userId, userEmail) : await RegisterNewUser(new User(gitHubResponse, userEmail));
        }

        /// <summary>
        /// Validates the provided Linkedin's id_token. If valid, it can return an existing user via Linkedin's user email response 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the id_token is invalid.
        /// </summary>
        /// <param name="idToken">The Linkedin's acess_token.</param>
        /// <returns>Existing user data or create new data after registering.</returns>
        private async Task<User?> HandleLinkedinIdToken(string idToken)
        {
            JwtSecurityToken? jwtResponse = await _linkedinAuthService.ValidateLinkedinIdToken(idToken);

            if (jwtResponse == null)
                return null;

            string userEmail = _linkedinAuthService.GetUserEmailFromJwt(jwtResponse);
            Guid userId = await GetUserIdByPlatformOrEmail(PlatformEnum.Linkedin, jwtResponse.Subject, userEmail);
            
            return userId.IsNotEmpty() ? User.FromLinkedinIdentity(jwtResponse, userId, userEmail) : await RegisterNewUser(new User(jwtResponse, jwtResponse.Subject, userEmail));
        }

        /// <summary>
        /// Returns the user id by platform and platformid or from userEmail
        /// If the user id is found by email, it will insert a new user platform access on table USERS_PROFILE_PLATFORM_DATA
        /// </summary>
        /// <param name="platform">Auth platform</param>
        /// <param name="platformId">Auth platform id</param>
        /// <param name="userEmail">User email returned by platform auth.</param>
        /// <returns>User Id on database or Guid.Empty.</returns>
        private async Task<Guid> GetUserIdByPlatformOrEmail(PlatformEnum platform, string platformId, string userEmail)
        {
            Guid userId = await _loginRepository.GetUserIdByPlatformId(platform, platformId);

            if (userId.IsEmpty())
            {
                userId = await _loginRepository.GetUserIdByEmail(userEmail);

                if (userId.IsNotEmpty())
                    await _signUpRepository.InsertUserPlatformData(userId, platform, platformId);
            }

            return userId;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user">New User object</param>
        /// <returns>A new user already inserted on database.</returns>
        private async Task<User> RegisterNewUser(User user)
        {
            return await _signUpRepository.RegisterUserByPlatform(user);
        }
        #endregion
    }
}
