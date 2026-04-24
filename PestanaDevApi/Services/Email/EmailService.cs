using System.Net.Mail;
using PestanaDevApi.Models;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Constants;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Exceptions;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Utils;
using System.Net;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IConfiguration _config;
        private readonly IConfirmationCodeService _confirmationCodeGenerationService;
        private readonly ITokenService _tokenService;

        private readonly string _emailAddress;
        private readonly string _appPassword;
        private readonly string _smtp;

        public EmailService(IConfiguration configuration, IEmailTemplateService emailTemplateService, IConfirmationCodeService codeGenerationService, ITokenService tokenService)
        {
            _config = configuration;
            _emailTemplateService = emailTemplateService;   
            _confirmationCodeGenerationService = codeGenerationService;
            _tokenService = tokenService;

            if (string.IsNullOrEmpty(_config["email.address"]))
                throw new InvalidOperationException(ErrorMessages.EmailAddress);

            if (string.IsNullOrEmpty(_config["email.apppassword"]))
                throw new InvalidOperationException(ErrorMessages.EmailAppPassword);

            if (string.IsNullOrEmpty(_config["email.smtp"]))
                throw new InvalidOperationException(ErrorMessages.EmailSmtp);

            _emailAddress = _config["email.address"]!;
            _appPassword = _config["email.apppassword"]!;
            _smtp = _config["email.smtp"]!;
        }

        public async Task<EmailResponse> SendContactEmail(ContactEmailRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.ClientEmail))
                return new EmailResponse(HttpStatusCode.BadRequest, ErrorMessages.InvalidEmailFormat);

            await Task.WhenAll(
            SendContactEmailToAdmin(request),
            SendContactEmailToClient(request));

            return new();
        }

        public async Task<SendConfirmationCodeEmailResponseDto> SendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.ClientEmail))
                return new SendConfirmationCodeEmailResponseDto(HttpStatusCode.BadRequest, ErrorMessages.InvalidEmailFormat);

            if (await _confirmationCodeGenerationService.CheckIfConfirmationCodeEmailAlreadySended(request.ClientEmail))
                return new SendConfirmationCodeEmailResponseDto(HttpStatusCode.BadRequest, ErrorMessages.EmailAlreadySended);

            string code = await _confirmationCodeGenerationService.GenerateConfirmationCode(request.ClientEmail);

            await SendSignUpCodeEmail(request, code);

            return new (resentJwt: _tokenService.GenerateResendConfirmationCodeJwt(request, code));
        }

        public async Task<SendConfirmationCodeEmailResponseDto> ResendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.ClientEmail))
                return new SendConfirmationCodeEmailResponseDto(HttpStatusCode.BadRequest, ErrorMessages.InvalidEmailFormat);

            if (!await _confirmationCodeGenerationService.CheckIfConfirmationCodeEmailAlreadySended(request.ClientEmail))
                return new SendConfirmationCodeEmailResponseDto(HttpStatusCode.BadRequest, ErrorMessages.EmailNotSended);

            string code = await _confirmationCodeGenerationService.GenerateConfirmationCode(request.ClientEmail, isResend: true);

            await SendSignUpCodeEmail(request, code);

            return new(resentJwt: _tokenService.GenerateResendConfirmationCodeJwt(request, code));
        }

        #region Private Methods
        private async Task SendEmail(MailMessage mailMessage)
        {
            using SmtpClient smtp = new ApiSmtpClient(_smtp, _emailAddress, _appPassword);

            try
            {
                await smtp.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new ApiException(ErrorMessages.EmailSendingError, 500, ex);
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessages.EmailUnespectedError, 500, ex);
            }
        }

        private async Task SendContactEmailToAdmin(ContactEmailRequestDto request)
        {
            using MailMessage mail = new ApiEmailMessage(_emailAddress, await _emailTemplateService.GetEmailTemplate(request));

            await SendEmail(mail);
        }

        private async Task SendContactEmailToClient(ContactEmailRequestDto request)
        {
            using MailMessage mail = new ApiEmailMessage(request, _emailAddress, await _emailTemplateService.GetEmailTemplate(request, isContactConfirmation: true));

            await SendEmail(mail);
        }

        private async Task SendSignUpCodeEmail(ConfirmationCodeEmailRequestDto request, string confirmationCodes)
        {
            using MailMessage mail = new ApiEmailMessage(request, _emailAddress, await _emailTemplateService.GetEmailTemplate(request, confirmationCodes));

            await SendEmail(mail);
        }
        #endregion
    }
}
