using PestanaDevApi.Constants;
using PestanaDevApi.Constants.Email;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Extensions.Enums
{
    public static class ConfirmationCodeEmailTypeExtensions
    {
        public static string ToSubCopyRight(this ConfirmationCodeEmailTypeEnum confirmationCode, string locale)
        {
            if (locale == LocaleConstants.En)
            {
                return confirmationCode switch
                {
                    ConfirmationCodeEmailTypeEnum.Contact => EmailContent.SubCopyRightContactConfirmationEnglish,
                    ConfirmationCodeEmailTypeEnum.SignUp => EmailContent.SubCopyRightSignUpConfirmationCodeEnglish,
                    ConfirmationCodeEmailTypeEnum.ForgotPassword => EmailContent.SubCopyRightForgotPasswordConfirmationCodeEnglish,
                    ConfirmationCodeEmailTypeEnum.CredentialsChange => EmailContent.SubCopyRightChangeCredentialsConfirmationCodeEnglish,
                    ConfirmationCodeEmailTypeEnum.DeleteAccount => EmailContent.SubCopyRightDeleteAccountConfirmationCodeEnglish,
                    _ => EmailContent.SubCopyRightContactConfirmationEnglish
                };
            }

            return confirmationCode switch
            {
                ConfirmationCodeEmailTypeEnum.Contact => EmailContent.SubCopyRightContactConfirmationPortuguese,
                ConfirmationCodeEmailTypeEnum.SignUp => EmailContent.SubCopyRightConfirmationCodeSignUpPortuguese,
                ConfirmationCodeEmailTypeEnum.ForgotPassword => EmailContent.SubCopyRightForgotPasswordConfirmationPortuguese,
                ConfirmationCodeEmailTypeEnum.CredentialsChange => EmailContent.SubCopyRightChangeCredentialsPortuguese,
                ConfirmationCodeEmailTypeEnum.DeleteAccount => EmailContent.SubCopyRightDeleteAccountConfirmationCodePortuguese,
                _ => EmailContent.SubCopyRightContactConfirmationPortuguese
            };
        }
    }
}
