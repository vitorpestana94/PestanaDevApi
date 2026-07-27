namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string EmailAlreadyBeingUsed = "8F2A : The provided email is already in use.";
        public const string EmailNotConfirmed = "0CEE : The provided email was not confirmed.";
        public const string InvalidEmailFormat = "A0B5 : The provided email format is invalid.";
        public const string EmailAlreadySended = "D7C1 : The provided email was already sended.";
        public const string UserSignUpWithPlatform = $"3G5F : The user signed up using a platform and can not redifine the password.";
        public const string EmailNotSended = "5BE9 : The provided email was not sended.";
        public const string EmailResentRequestedTooSoon = "C8A4 : The request to send a new confirmation code email was done too soon.";
        public const string EmailSendingError = "F19D : An error occurred when communicating with the email server!";
        public const string EmailUnespectedError = "2E6B : An unespected error ocurred while sending the email!";
        public const string EmailDoestNotExists = "91AF : The provided email does not exists on our database!";
        public const string UserEmailWasNotConfirmed = "7D3E : The user email was not confirmed!";
    }
}
