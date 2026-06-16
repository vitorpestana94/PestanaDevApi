using System.Net;
using PestanaDevApi.Constants;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Services
{
    public class ForgotPasswordService: IForgotPasswordService
    {
        private readonly IConfirmedEmailsRepository _confirmedEmailsRepository;
        private readonly IForgotPasswordRepository _forgetPasswordRepository;
        private readonly ICaptchaService _captchaService;

        public ForgotPasswordService(IConfirmedEmailsRepository confirmedEmailsRepository, IForgotPasswordRepository forgotPasswordRepository, ICaptchaService captchaService) 
        {
            _confirmedEmailsRepository = confirmedEmailsRepository;
            _forgetPasswordRepository = forgotPasswordRepository;
            _captchaService = captchaService;
        }

        public async Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request)
        {
            if (!await _captchaService.ValidateCaptchaV3(request.CaptchaToken))
                return new(HttpStatusCode.Forbidden, ErrorMessages.UserBeheaviorItsNotHuman);

            if (!ApiLib.IsEmailValid(request.Email))
                return new(ErrorMessages.InvalidEmailFormat);

            if (!await _confirmedEmailsRepository.IsEmailConfirmed(request.Email))
                return new(ErrorMessages.EmailNotConfirmed);

            await _forgetPasswordRepository.ChangeUserPassword(request.Email, password: HashFactory.HashPassword(request.NewPassword));

            await _confirmedEmailsRepository.DeleteEmailConfirmation(request.Email);

            return new();
        } 
    }
}
