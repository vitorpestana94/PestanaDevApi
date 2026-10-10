using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Constants.Email;
using Keys = PestanaDevApi.Constants.Email.EmailTranslationKeys;
using Locale = PestanaDevApi.Constants.LocaleConstants;

namespace PestanaDevApi.Services.Email
{
    public class EmailTemplateService: IEmailTemplateService
    {
        private readonly string _templatesDirectory;
        private readonly ITranslationService _translationService;
        private static readonly string[] _confirmationCodesVariables = [EmailConstants.CodeOne, EmailConstants.CodeTwo, EmailConstants.CodeThree, EmailConstants.CodeFour];

        public EmailTemplateService(ITranslationService translationService)
        {
            _templatesDirectory = EmailConstants.TemplatesDirectory;
            _translationService = translationService;
        }

        public async Task<string> GetEmailTemplate(ContactEmailRequestDto request, bool isContactEmailClientConfirmation = false)
        {
            string emailTemplate = await GetEmailTemplateString(isContactEmailClientConfirmation ? EmailTemplateNameEnum.ContactEmailClientConfirmation : EmailTemplateNameEnum.ContactEmail);

            return isContactEmailClientConfirmation ?
                ReplaceClientContactVariables(request, emailTemplate) :
                ReplaceAdminContactVariables(request, emailTemplate);
        }

        public async Task<string> GetEmailTemplate(ConfirmationCodeEmailRequestDto request, string confirmationCodes)
        {
            string emailTemplate = await GetEmailTemplateString(EmailTemplateNameEnum.CodeConfirmationEmail);

            return ReplaceConfirmationCodeVariables(request, emailTemplate, confirmationCodes);
        }

        #region Private Methods
        private async Task<string> GetEmailTemplateString(EmailTemplateNameEnum templateName)
        {
            return await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), _templatesDirectory, EmailConstants.GetTemplateHtml(templateName)));
        }

        /// <summary>
        /// The admin email is always sent in english, since it goes to the website owner.
        /// </summary>
        private string ReplaceAdminContactVariables(ContactEmailRequestDto request, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.SubjectVariable, EmailConstants.ContactEmailSubject)
                                .Replace(EmailConstants.ClientEmailVariable, request.ClientEmail)
                                .Replace(EmailConstants.ClientNameVariable, request.ClientName)
                                .Replace(EmailConstants.ClientMessageVariable, request.ClientMessage)
                                .Replace(EmailConstants.Copyright, GetCopyright(Locale.En))
                                .Replace(EmailConstants.Subcopyright, Translate(Locale.En, Keys.ContactAdminSubcopyright));
        }

        private string ReplaceClientContactVariables(ContactEmailRequestDto request, string emailTemplate)
        {
            string locale = request.ClientLocale;

            return emailTemplate.Replace(EmailConstants.Title, Translate(locale, Keys.ContactClientTitle, new() { ["name"] = request.ClientName }))
                                .Replace(EmailConstants.Paragraph, Translate(locale, Keys.ContactClientParagraph))
                                .Replace(EmailConstants.Copyright, GetCopyright(locale))
                                .Replace(EmailConstants.Subcopyright, Translate(locale, Keys.ContactClientSubcopyright));
        }

        private string ReplaceConfirmationCodeVariables(ConfirmationCodeEmailRequestDto request, string emailTemplate, string confirmationCodes)
        {
            string locale = request.ClientLocale;

            foreach ((string codeVariable, char codeValue) in _confirmationCodesVariables.Zip(confirmationCodes))
            {
                emailTemplate = emailTemplate.Replace(codeVariable, codeValue.ToString());
            }

            return emailTemplate.Replace(EmailConstants.Title, Translate(locale, Keys.ConfirmationCodeTitle))
                                .Replace(EmailConstants.Paragraph, Translate(locale, Keys.ConfirmationCodeParagraph))
                                .Replace(EmailConstants.Copyright, GetCopyright(locale))
                                .Replace(EmailConstants.Subcopyright, Translate(locale, Keys.GetConfirmationCodeSubcopyright(request.ConfirmationCodeEmailType)));
        }

        private string GetCopyright(string locale) => Translate(locale, Keys.Copyright, new() { ["year"] = DateTime.Now.Year.ToString() });

        private string Translate(string locale, string key, Dictionary<string, string>? values = null) => _translationService.Translate(locale, key, values);
        #endregion
    }
}
