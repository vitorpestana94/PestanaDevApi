namespace PestanaDevApi.Constants
{
    public static partial class ErrorMessages
    {
        public const string EmailAlreadyBeingUsed = "The provided email is already in use.";
        public const string EmailNotConfirmed = "The provided email was not confirmed.";
        public const string InvalidEmailFormat = "The provided email format is invalid.";
        public const string EmailAlreadySended = "The provided email was already sended.";
        public const string EmailNotSended = "The provided email was not sended.";
        public const string EmailResentRequestedTooSoon = "The request to send a new confirmation code email was done too soon.";
        public const string EmailSendingError = "An error occurred when communicating with the email server!";
        public const string EmailUnespectedError = "An unespected error ocurred while sending the email!";
        public const string EmailDoestNotExists = "The provided email does not exists on our database!";
        public const string UserEmailWasNotConfirmed = "The user email was not confirmed!";
    }
}
