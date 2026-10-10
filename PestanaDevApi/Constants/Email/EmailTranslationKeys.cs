using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Constants.Email
{
    /// <summary>
    /// Keys of Locales/{locale}/email.json.
    /// </summary>
    public static class EmailTranslationKeys
    {
        public const string Copyright = "email:copyright";

        #region Contact
        public const string ContactAdminSubcopyright = "email:contact.admin.subcopyright";
        public const string ContactClientTitle = "email:contact.client.title";
        public const string ContactClientParagraph = "email:contact.client.paragraph";
        public const string ContactClientSubcopyright = "email:contact.client.subcopyright";
        #endregion

        #region Confirmation Code
        public const string ConfirmationCodeTitle = "email:confirmationCode.title";
        public const string ConfirmationCodeParagraph = "email:confirmationCode.paragraph";
        public static string GetConfirmationCodeSubcopyright(ConfirmationCodeEmailTypeEnum type) => $"email:confirmationCode.subcopyright.{type}";
        #endregion
    }
}
