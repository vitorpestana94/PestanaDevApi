using PestanaDevApi.Constants.Email;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Utils
{
    public static class ReplaceEmailVariables
    {
        public static string ReplaceVariables(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return requestDto.ClientLocale switch
            {
                "en" => EnglishReplace(requestDto, emailTemplate),
                "pt" => PortugueseReplace(requestDto, emailTemplate),
                _ => EnglishReplace(requestDto, emailTemplate),
            };
        }

        public static string ReplaceAdminEmailContactVariables(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.SubjectVariable, EmailConstants.ContactEmailSubject)
                                .Replace(EmailConstants.ClientEmailVariable, requestDto.ClientEmail)
                                .Replace(EmailConstants.ClientNameVariable, requestDto.ClientName)
                                .Replace(EmailConstants.ClientMessageVariable, requestDto.ClientMessage)
                                .Replace(EmailConstants.Copyright, $"{GetCurrentYear()}{EmailContent.CopyRightEnglish}")
                                .Replace(EmailConstants.Subcopyright, EmailContent.CopyRightEnglish);
        }

        #region Private Methods
        private static string EnglishReplace(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.Title, $"{EmailContent.ContactConfirmationEmailTitleEnglish}{requestDto.ClientName}!")
                                .Replace(EmailConstants.Paragraph, EmailContent.ContactConfirmationEmailParagraphEnglish)
                                .Replace(EmailConstants.Copyright, $"{GetCurrentYear()}{EmailContent.CopyRightEnglish}")
                                .Replace(EmailConstants.Subcopyright, EmailContent.SubCopyRightContactConfirmationEnglish);
        }

        private static string PortugueseReplace(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.Title, $"{EmailContent.ContactConfirmationEmailTitlePortuguese}{requestDto.ClientName}!")
                                .Replace(EmailConstants.Paragraph, EmailContent.ContactConfirmationEmailParagraphPortuguese)
                                .Replace(EmailConstants.Copyright, $"{GetCurrentYear()}{EmailContent.CopyRightPortuguese}")
                                .Replace(EmailConstants.Subcopyright, EmailContent.SubCopyRightContactConfirmationPortuguese);
        }

        private static string GetCurrentYear()
        {
            return DateTime.Now.Year.ToString();
        }
        #endregion
    }
}
