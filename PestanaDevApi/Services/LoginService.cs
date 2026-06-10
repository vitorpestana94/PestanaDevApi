using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Utils;
using PestanaDevApi.Constants;
using System.Net;
using PestanaDevApi.Interfaces.Services.Auth;

namespace PestanaDevApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ITokenService _tokenService;
        private readonly IPlatformAuthService _platformAuthService;

        public LoginService(ILoginRepository loginRepository, ITokenService tokenService, IPlatformAuthService platformAuthService)
        {
            _loginRepository = loginRepository;
            _tokenService = tokenService;
            _platformAuthService = platformAuthService;
        }

        /// <summary>
        /// Login flow.
        /// <param name="request">The login request.</param>
        /// <returns>Both JWT token and refresh token.</returns>
        /// </summary>
        public async Task<AuthResponseDto> Login(LoginRequestDto request)
        {
            User? user = await GetUserByEmail(request.Email);

            if (user == null)
                return new(ErrorMessages.InvalidCredentials);

            if (user.SignupByPlatform)
                return new(ErrorMessages.InvalidLoginEndpointUserWithPlatform);

            if (user == null || PasswordVerifier.IsPasswordNotValid(dtoPassword: request.Password, userPassword: user.UserPassword))
                return new(ErrorMessages.InvalidCredentials);

            return new(await _tokenService.GenerateApiTokens(user, request.DeviceId));
        }

        /// <summary>
        /// Authenticates or registers a user via an external provider. 
        /// If the user does not exist in the local database, a new record is created (Sign-Up).
        /// If the user already exists, the session is initialized (Log-In).
        /// </summary>
        /// <param name="request">The request data containing the provider's token and a enum that indicates the provider.</param>
        /// <returns>A task representing the operation, yielding the authentication tokens.</returns>
        public async Task<AuthResponseDto> LoginOrSignUpWithProvider(LoginOrSignUpWithPlatformRequestDto request)
        {
            User? user = await _platformAuthService.GetUserByIoken(request.Token, request.Platform);

            if (user == null) // If the user is null, it means that the provided token is not valid for the requested platform.
                return new(HttpStatusCode.Unauthorized);

            if (!user.SignupByPlatform) // This will probably never happen here; but this line of code is here as a safeguard.
                return new(ErrorMessages.InvalidLoginEndpointUserWithPassword);

            return new(await _tokenService.GenerateApiTokens(user, request.DeviceId));
        }

        #region Private Methods
        /// <summary>
        /// Get a user profile properties if exists.
        /// <param name="email">The user's email.</param>
        /// <returns>Both JWT token and refresh token.</returns>
        /// <exception cref="UnauthorizedException">Thrown if there's no registered user with the provided email.</exception>
        /// </summary>
        private async Task<User?> GetUserByEmail(string email)
        {
            if (!ApiLib.IsEmailValid(email))
                return null;

            return await _loginRepository.GetUserDataByEmail(email);
        }
        #endregion
    }
}
