using PestanaDevApi.Dtos.Requests;
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

        public SignUpService(ISignUpRepository signUpRepository, ITokenService tokenService)
        {
            _signUpRepository = signUpRepository;
            _tokenService = tokenService;
        }

        public async Task<ApiToken?> SignUp(SignUpRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.Email))
                return null;
    
            return await _tokenService.GenerateApiTokens(await _signUpRepository.InsertUser(new User(request)), request.DeviceId);
        }
    }
}
