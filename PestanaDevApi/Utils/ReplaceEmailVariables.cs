using PestanaDevApi.Constants.Email;
using PestanaDevApi.Dtos.Requests;
using Locale = PestanaDevApi.Constants.LocaleConstants;

namespace PestanaDevApi.Utils
{
    public static class ReplaceEmailVariables
    {
        private static readonly string[] _confirmationCodesVariables = [EmailConstants.CodeOne, EmailConstants.CodeTwo, EmailConstants.CodeThree, 
        EmailConstants.CodeFour, EmailConstants.CodeFive, EmailConstants.CodeSix, EmailConstants.CodeSeven, EmailConstants.CodeEight];
        private static readonly string _copyRight = $"{GetCurrentYear()}{EmailContent.CopyRightEnglish}";

        #region Contact Email
        public static string ReplaceClientContactVariables(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return requestDto.ClientLocale switch
            {
                Locale.En => EnglishReplace(requestDto, emailTemplate),
                Locale.Pt => PortugueseReplace(requestDto, emailTemplate),
                _ => EnglishReplace(requestDto, emailTemplate),
            };
        }

        public static string ReplaceAdminEmailContactVariables(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.SubjectVariable, EmailConstants.ContactEmailSubject)
                                .Replace(EmailConstants.ClientEmailVariable, requestDto.ClientEmail)
                                .Replace(EmailConstants.ClientNameVariable, requestDto.ClientName)
                                .Replace(EmailConstants.ClientMessageVariable, requestDto.ClientMessage)
                                .Replace(EmailConstants.Copyright, _copyRight)
                                .Replace(EmailConstants.Subcopyright, EmailContent.CopyRightEnglish);
        }
        #endregion

        #region Confirmation Code
        public static string ReplaceConfirmationCodetVariables(ConfirmationCodeEmailRequestDto requestDto, string emailTemplate, string confirmationCodes)
        {
            return requestDto.ClientLocale switch
            {
                Locale.En => EnglishReplace(emailTemplate, confirmationCodes),
                Locale.Pt => PortugueseReplace(emailTemplate, confirmationCodes),
                _ => EnglishReplace(emailTemplate, confirmationCodes)
            };
        }
        #endregion

        #region Private Methods

        #region Contact Email
        private static string EnglishReplace(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.Title, $"{EmailContent.ContactConfirmationEmailTitleEnglish}{requestDto.ClientName}!")
                                .Replace(EmailConstants.Paragraph, EmailContent.ContactConfirmationEmailParagraphEnglish)
                                .Replace(EmailConstants.Copyright, _copyRight)
                                .Replace(EmailConstants.Subcopyright, EmailContent.SubCopyRightContactConfirmationEnglish);
        }

        private static string PortugueseReplace(ContactEmailRequestDto requestDto, string emailTemplate)
        {
            return emailTemplate.Replace(EmailConstants.Title, $"{EmailContent.ContactConfirmationEmailTitlePortuguese}{requestDto.ClientName}!")
                                .Replace(EmailConstants.Paragraph, EmailContent.ContactConfirmationEmailParagraphPortuguese)
                                .Replace(EmailConstants.Copyright, _copyRight)
                                .Replace(EmailConstants.Subcopyright, EmailContent.SubCopyRightContactConfirmationPortuguese);
        }
        #endregion
        
        #region Confirmation Code
        private static string EnglishReplace(string emailTemplate, string confirmationCodes)
        {
            emailTemplate = GetEmailTemplateWithCodes(emailTemplate, confirmationCodes);

            return emailTemplate.Replace(EmailConstants.Title, $"{EmailContent.ConfirmationCodeTitleEnglish}")
                                .Replace(EmailConstants.Paragraph, EmailContent.ConfirmationCodeTitleParagraphEnglish)
                                .Replace(EmailConstants.Copyright, _copyRight)
                                .Replace(EmailConstants.Subcopyright, EmailContent.SubCopyRightContactConfirmationEnglish);
        }

        private static string PortugueseReplace(string emailTemplate, string confirmationCodes)
        {
            emailTemplate = GetEmailTemplateWithCodes(emailTemplate, confirmationCodes);

            return emailTemplate.Replace(EmailConstants.Title, $"{EmailContent.ConfirmationCodeTitlePortuguese}")
                                .Replace(EmailConstants.Paragraph, EmailContent.ConfirmationCodeTitleParagraphPortuguese)
                                .Replace(EmailConstants.Copyright, _copyRight)
                                .Replace(EmailConstants.Subcopyright, EmailContent.SubCopyRightContactConfirmationPortuguese);
        }

        #endregion

        private static string GetEmailTemplateWithCodes(string emailTemplate, string confirmationCodes)
        {
            IEnumerable<(string codeVariable, char codeValue)> codesVariablesWithValues = _confirmationCodesVariables.Zip(confirmationCodes);

            foreach ((string codeVariable, char codeValue) in codesVariablesWithValues)
            {
                emailTemplate = emailTemplate.Replace(codeVariable, codeValue.ToString());
            }

            return emailTemplate;
        }

        private static string GetCurrentYear() => DateTime.Now.Year.ToString();
        #endregion
    }
}
