using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using Hash = PestanaDevApi.Utils.SHA526Factory;
using PestanaDevApi.Utils;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Constants;
using System.Net;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Services
{
    public class ConfirmationCodeService: IConfirmationCodeService
    {
        private readonly string _secretKey; 
        private readonly IConfiguration _config;
        private readonly IConfirmationCodeGenerationRepository _repository;

        public ConfirmationCodeService(IConfirmationCodeGenerationRepository repository, IConfiguration configuration)
        {
            _config = configuration;

            if (string.IsNullOrEmpty(_config["sha.secret"]))
                throw new InvalidOperationException(ErrorMessages.EmailAddress);

            _secretKey = _config["sha.secret"]!;

            _repository = repository;
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

        public async Task<bool> CheckIfConfirmationCodeEmailAlreadySended(string email)
        {
            return await _repository.SelectOneIfTheresEmail(email);
        }

        public async Task<CheckConfirmationCodeResponse> IsConfirmationCodeValid(CheckConfirmationCodeRequest request)
        {
            if (!await _repository.SelectOneIfTheresEmail(request.ClientEmail))
                return new CheckConfirmationCodeResponse(HttpStatusCode.BadRequest, ErrorMessages.EmailDoestNotExists);

            if (!await _repository.SelectOneIfCodeIsStillFresh(request.ClientEmail))
                return new CheckConfirmationCodeResponse(HttpStatusCode.BadRequest, ErrorMessages.UnfreshCode);

            if (!await ValidateCode(request.ClientEmail, request.Code))
                return new CheckConfirmationCodeResponse(HttpStatusCode.BadRequest, ErrorMessages.InvalidCode);

            await _repository.DeleteConfirmationCode(request.ClientEmail);

            return new();
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
