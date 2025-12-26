using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Exceptions;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ITokenService _tokenService;

        public LoginService(ILoginRepository loginRepository, ITokenService tokenService)
        {
            _loginRepository = loginRepository;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Inserts a new refresh token.
        /// <param name="request">The login request contains the user's email, password and device ID (a UUID used to generate a refresh token for each device).</param>
        /// <returns>Both JWT token and refresh token.</returns>
        /// <exception cref="UnauthorizedException">Thrown when the provided password and the stored hash do not match.</exception>
        /// </summary>
        public async Task<ApiToken> Login(LoginRequestDto request)
        {
            User user = await GetUserByEmail(request.Email);

            if (IsPasswordNotValid(request.Senha, user.UserPassword))
                throw new UnauthorizedException("Invalid Credentials");

            return await _tokenService.GenerateApiTokens(user.Id, request.DeviceId);
        }

        #region Private Methods
        /// <summary>
        /// Get a user profile properties if exists.
        /// <param name="email">The user's email.</param>
        /// <returns>Both JWT token and refresh token.</returns>
        /// <exception cref="UnauthorizedException">Thrown if there's no registered user with the provided email.</exception>
        /// </summary>
        private async Task<User> GetUserByEmail(string email)
        {
            ApiLib.IsEmailValid(email);

            return await _loginRepository.GetUserDataByEmail(email) ?? throw new UnauthorizedException();
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
