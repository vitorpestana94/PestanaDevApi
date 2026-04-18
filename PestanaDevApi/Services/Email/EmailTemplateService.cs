using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Constants.Email;
using Replace = PestanaDevApi.Utils.ReplaceEmailVariables;

namespace PestanaDevApi.Services.Email
{
    public class EmailTemplateService: IEmailTemplateService
    {
        private readonly string _templatesDirectory;

        public EmailTemplateService() 
        {
            _templatesDirectory= EmailConstants.TemplatesDirectory;
        }

        public async Task<string> GetEmailTemplate(ContactEmailRequestDto request, bool isContactEmailClientConfirmation = false)
        {
            string emailTemplate = await GetEmailTemplateString(isContactEmailClientConfirmation ? EmailTemplateName.ContactEmailClientConfirmation : EmailTemplateName.ContactEmail);

            return ReplaceEmailVariables(request, isContactEmailClientConfirmation,  emailTemplate);
        }

        public async Task<string> GetEmailTemplate(ConfirmationCodeEmailRequestDto request, string confirmationCodes)
        {
            string emailTemplate = await GetEmailTemplateString(EmailTemplateName.SignUpCodeConfirmationEmail);

            return Replace.ReplaceConfirmationCodetVariables(request, emailTemplate, confirmationCodes); ;
        }

        #region Private Methods
        private async Task<string> GetEmailTemplateString(EmailTemplateName templateName)
        {
            return await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), _templatesDirectory, $"{templateName}.html"));
        }

        private static string ReplaceEmailVariables(ContactEmailRequestDto requestDto, bool isContactEmailClientConfirmation, string emailTemplate)
        {
            return isContactEmailClientConfirmation ?
                Replace.ReplaceClientContactVariables(requestDto, emailTemplate):
                Replace.ReplaceAdminEmailContactVariables(requestDto, emailTemplate); 
        }

        #endregion
    }
}
