using System.Net;
using PestanaDevApi.Constants.Messages;
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
        private readonly IConfirmedEmailsRepository _confirmedEmailsRepository;

        public SignUpService(ISignUpRepository signUpRepository, ITokenService tokenService, IConfirmedEmailsRepository confirmedEmailsRepository)
        {
            _signUpRepository = signUpRepository;
            _tokenService = tokenService;
            _confirmedEmailsRepository = confirmedEmailsRepository;
        }

        public async Task<SignUpResponseDto> SignUp(SignUpRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.Email))
                return new(ErrorMessages.InvalidEmailFormat);

            if (await _signUpRepository.IsEmailBeingUsed(request.Email))
                return new(ErrorMessages.EmailAlreadyBeingUsed);

            if (!await _confirmedEmailsRepository.IsEmailConfirmed(request.Email))
                return new(HttpStatusCode.Forbidden, ErrorMessages.EmailNotConfirmed);

            User newUser = await _signUpRepository.RegisterUser(new User(request));

            await _confirmedEmailsRepository.DeleteEmailConfirmation(request.Email);

            return new (await _tokenService.GenerateApiTokens(newUser));
        }

        public async Task<IsEmailAlreadyRegisteredResponseDto> IsEmailAlreadyRegistered(string email)
        {
            if (!ApiLib.IsEmailValid(email))
                return new(ErrorMessages.InvalidEmailFormat);

            return new (await _signUpRepository.IsEmailBeingUsed(email));
        }
    }
}
