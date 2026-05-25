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

        public ForgotPasswordService(IConfirmedEmailsRepository confirmedEmailsRepository, IForgotPasswordRepository forgotPasswordRepository) 
        {
            _confirmedEmailsRepository = confirmedEmailsRepository;
            _forgetPasswordRepository = forgotPasswordRepository;
        }

        public async Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request)
        {
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
