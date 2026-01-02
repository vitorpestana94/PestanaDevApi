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
        private readonly IPlatformAuthService _platformAuthService;

        public SignUpService(ISignUpRepository signUpRepository, ITokenService tokenService, IPlatformAuthService platformAuthService)
        {
            _signUpRepository = signUpRepository;
            _tokenService = tokenService;
            _platformAuthService = platformAuthService;
        }

        public async Task<SignUpResponseDto> SignUp(SignUpRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.Email))
                return new(ErrorMessages.InvalidEmailFormat);

            if(!await _signUpRepository.IsEmailBeingUsed(request.Email))
                return new(ErrorMessages.EmailAlreadyBeingUsed);

            return new(await _tokenService.GenerateApiTokens(user: await _signUpRepository.RegisterUser(new User(request)), deviceId: request.DeviceId));
        }

        //public async Task<SignUpResponseDto> SignUpWithPlatform(SignUpWithPlatformRequestDto request)
        //{
        //    return new(await _tokenService.GenerateApiTokens(user: await _signUpRepository.RegisterUser(new User(request)), deviceId: request.DeviceId));
        //}
    }
}
