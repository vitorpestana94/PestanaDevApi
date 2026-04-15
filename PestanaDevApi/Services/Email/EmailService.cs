using System.Net.Mail;
using PestanaDevApi.Models;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Constants;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Exceptions;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Utils;
using System.Net;

namespace PestanaDevApi.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IConfiguration _config;
        private readonly string _emailAddress;
        private readonly string _appPassword;
        private readonly string _smtp;

        public EmailService(IConfiguration configuration, IEmailTemplateService emailTemplateService, ILogger<EmailService> logger)
        {
            _config = configuration;
            _emailTemplateService = emailTemplateService;   
            _logger = logger;
            
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

        public async Task<EmailResponse> SendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request)
        {
            if (!ApiLib.IsEmailValid(request.ClientEmail))
                return new EmailResponse(HttpStatusCode.BadRequest, ErrorMessages.InvalidEmailFormat);

            // aqui chamarei a service para gerar o código que será enviado no email
            // essa service vai também fazer um insert do código, para ser verificado depois.
            // aproveitando, é preciso esclarecer se  _logger.LogError(ex, ErrorMessages.EmailSendingError); é perigoso
            // se mostra algo no console q n deveria e se a forma q eu trato erros está ok (quase certeza q s); isso pode ser visto depois.
            // depois preciso lembrar de me livrar de qualquer menção no codigo e na database acerca de foto de perfil do usuario por causa da lgpd
            await SendSignUpCodeEmail(request, [123131]);

            return new();
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
                _logger.LogError(ex, ErrorMessages.EmailSendingError);

                throw new ApiException(ErrorMessages.EmailSendingError, 500, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.EmailUnespectedError);

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

        private async Task SendSignUpCodeEmail(ConfirmationCodeEmailRequestDto request, IEnumerable<int> confirmationCodes)
        {
            using MailMessage mail = new ApiEmailMessage(request, _emailAddress, await _emailTemplateService.GetEmailTemplate(request, confirmationCodes));

            await SendEmail(mail);
        }
        #endregion
    }
}
