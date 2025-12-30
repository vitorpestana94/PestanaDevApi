using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Utils;
using PestanaDevApi.Constants;
using PestanaDevApi.Models.Enums;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace PestanaDevApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ISignUpRepository _signUpRepository;
        private readonly ITokenService _tokenService;
        private readonly IPlatformAuthService _platformAuthService;

        public LoginService(ILoginRepository loginRepository, ITokenService tokenService, ISignUpRepository signUpRepository, IPlatformAuthService platformAuthService)
        {
            _loginRepository = loginRepository;
            _tokenService = tokenService;
            _signUpRepository = signUpRepository;
            _platformAuthService = platformAuthService;
        }

        /// <summary>
        /// Login flow.
        /// <param name="request">The login request.</param>
        /// <returns>Both JWT token and refresh token.</returns>
        /// </summary>
        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            User? user = await GetUserByEmail(request.Email);

            if (user == null || IsPasswordNotValid(request.Password, user.UserPassword))
                return new(ErrorMessages.InvalidCredentials);

            return new (await _tokenService.GenerateApiTokens(user, request.DeviceId));
        }

        /// <summary>
        /// Login flow with platforms, like Google.
        /// <param name="request">The login request.</param>
        /// <returns>Both JWT token and refresh token.</returns>
        /// </summary>
        public async Task<LoginResponseDto> LoginWithProvider(LoginWithPlatformRequestDto request)
        {
            User? user = await GetUserByIdToken(request.IdToken, request.Platform);

            if(user == null)
                return new(HttpStatusCode.Unauthorized);

            return new (await _tokenService.GenerateApiTokens(user, request.DeviceId));
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

        private async Task<User?> GetUserByIdToken(string idToken, Platform platform)
        {
            return platform switch
            {
                Platform.Google => await _platformAuthService.HandleGoogleIdToken(idToken),
                _ => null
            };
        }

        /// <summary>
        /// Get a user profile properties if exists.
        /// <param name="dtoPassword">The provided password on login request.</param>
        /// <param name="userPassword">The stored password hash.</param>
        /// <returns>True if the provided password matchs with the stored hash, false otherwise.</returns>
        /// </summary>
        private static bool IsPasswordNotValid(string dtoPassword, string userPassword)
        {
            return string.IsNullOrEmpty(userPassword) || !BCrypt.Net.BCrypt.Verify(dtoPassword, userPassword);
        }
        #endregion
    }
}
