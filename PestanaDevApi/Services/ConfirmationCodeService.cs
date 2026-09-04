using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using Hash = PestanaDevApi.Utils.SHA526Factory;
using PestanaDevApi.Utils;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Constants.Messages;
using System.Net;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Services
{
    public class ConfirmationCodeService: IConfirmationCodeService
    {
        private readonly string _secretKey; 
        private readonly IConfiguration _config;
        private readonly IConfirmationCodeGenerationRepository _repository;
        private readonly IConfirmedEmailsRepository _confirmedEmailsRepository;

        public ConfirmationCodeService(IConfirmationCodeGenerationRepository repository, IConfiguration configuration, IConfirmedEmailsRepository confirmedEmailsRepository)
        {
            _config = configuration;

            if (string.IsNullOrEmpty(_config["sha.secret"]))
                throw new InvalidOperationException(ErrorMessages.EmailAddress);

            _secretKey = _config["sha.secret"]!;

            _repository = repository;
            _confirmedEmailsRepository = confirmedEmailsRepository;
        }

        public async Task<string> GenerateConfirmationCode(string userEmail, bool isResend = false)
        {
            string randomCode = ApiLib.GenerateRandomCode();

            if (isResend)
                await _repository.UpdateConfirmationCode(userEmail, code: Hash.GenerateHmac(randomCode, _secretKey));
            else
                await _repository.InsertConfirmationCode(userEmail, code: Hash.GenerateHmac(randomCode, _secretKey));

            return randomCode;
        }

        public async Task<CheckIfConfirmationCodeEmailAlreadySentResponseDto> CheckConfirmationCodeEmailAlreadySent(string email)
        {
            if (!ApiLib.IsEmailValid(email))
                return new CheckIfConfirmationCodeEmailAlreadySentResponseDto(HttpStatusCode.BadRequest, ErrorMessages.InvalidEmailFormat);

            return new CheckIfConfirmationCodeEmailAlreadySentResponseDto(confirmationCodeAlreadySent: await CheckIfConfirmationCodeEmailAlreadySent(email));
        }

        public async Task<bool> CheckIfConfirmationCodeEmailAlreadySent(string email)
        {
            return await _repository.SelectOneIfTheresEmail(email);
        }

        public async Task<CheckConfirmationCodeResponse> IsConfirmationCodeValid(CheckConfirmationCodeRequestDto request)
        {
            if (!await _repository.SelectOneIfTheresEmail(request.ClientEmail))
                return new CheckConfirmationCodeResponse(HttpStatusCode.BadRequest, ErrorMessages.EmailDoestNotExists);

            if (!await _repository.SelectOneIfCodeIsStillFresh(request.ClientEmail))
                return new CheckConfirmationCodeResponse(HttpStatusCode.BadRequest, ErrorMessages.UnfreshCode);

            if (!await ValidateCode(request.ClientEmail, request.Code))
                return new CheckConfirmationCodeResponse(HttpStatusCode.Unauthorized, ErrorMessages.InvalidCode);

            if (!await _confirmedEmailsRepository.IsEmailConfirmed(request.ClientEmail))
            {
                await _confirmedEmailsRepository.RegisterEmailConfirmation(request.ClientEmail);

                await _repository.DeleteConfirmationCode(request.ClientEmail);
            }
            else
            {
                await _repository.DeleteConfirmationCode(request.ClientEmail);
            }

            return new();
        }

        public async Task<bool> CheckCreatedAt(string email)
        {
            return await _repository.CheckCreatedAt(email);
        }

        public async Task DeleteUnfreshConfirmationCodes()
        {
            await _repository.DeleteUnfreshConfirmationCodes();
        }

        #region Private Methods
        private async Task<bool> ValidateCode(string clientEmail, string code)
        {
            return Hash.ValidateCode(
                inputCode: code,
                storedHash: await _repository.SelectCodeByEmail(clientEmail),
                _secretKey);
        }
        #endregion
    }
}
