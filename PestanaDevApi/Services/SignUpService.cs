using PestanaDevApi.Constants;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly ISignUpRepository _signUpRepository;
        private readonly ITokenService _tokenService;
        private readonly ILoginService _loginService;
        public SignUpService(ISignUpRepository signUpRepository, ITokenService tokenService, ILoginService loginService)
        {
            _signUpRepository = signUpRepository;
            _tokenService = tokenService;
            _loginService = loginService;
        }

        public async Task<SignUpResponseDto> SignUp(SignUpRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.Email))
                return new(ErrorMessages.InvalidEmailFormat);

            if(await _signUpRepository.IsEmailBeingUsed(request.Email))
                return new(ErrorMessages.EmailAlreadyBeingUsed);

            return new(await _tokenService.GenerateApiTokens(user: await _signUpRepository.RegisterUser(new User(request)), deviceId: request.DeviceId));
        }

        public async Task<IsEmailAlreadyRegisteredResponseDto> IsEmailAlreadyRegistered(string email)
        {
            if (!ApiLib.IsEmailValid(email))
                return new(ErrorMessages.InvalidEmailFormat);

            return new(await _signUpRepository.IsEmailBeingUsed(email));
        }

        /// <summary>
        /// Authenticates or registers a user via an external provider. 
        /// If the user does not exist in the local database, a new record is created (Sign-Up).
        /// If the user already exists, the session is initialized (Log-In).
        /// </summary>
        /// <param name="request">The request data containing the provider's token and a enum that indicates the provider.</param>
        /// <returns>A task representing the operation, yielding the authentication tokens.</returns>
        public async Task<SignUpWithPlatformResponseDto> SignUpWithPlatform(SignUpWithPlatformRequestDto request)
        {
            return new SignUpWithPlatformResponseDto(await _loginService.LoginOrSignUpWithProvider(request));
        }
    }
}
