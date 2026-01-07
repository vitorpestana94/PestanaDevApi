using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Constants;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Services.Email
{
    public class EmailTemplateService: IEmailTemplateService
    {
        private readonly string _templatesDirectory;

        public EmailTemplateService(IWebHostEnvironment env) 
        {
            _templatesDirectory= EmailConstants.TemplatesDirectory;
        }

        public async Task<string> GetEmailTemplate(ContactEmailRequestDto request)
        {
            return ReplaceEmailVariables(request, emailTemplate: await File.ReadAllTextAsync(GetEmailTemplate(EmailTemplateName.ContactEmail)));
        }

        #region Private Methods
        private string GetEmailTemplate(EmailTemplateName templateName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), _templatesDirectory, GetTemplateName(templateName));
        }

        private static string GetTemplateName(EmailTemplateName templateName)
        {
            return $"{templateName}.html";
        }

        private static string ReplaceEmailVariables(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.SubjectVariable, EmailConstants.ContactEmailSubject)
                                .Replace(EmailConstants.ClientEmailVariable, requestDto.ClientEmail)
                                .Replace(EmailConstants.ClientNameVariable, requestDto.ClientName)
                                .Replace(EmailConstants.ClientMessageVariable, requestDto.ClientMessage);
        }
        #endregion
    }
}
