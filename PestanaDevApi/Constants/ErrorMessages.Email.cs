namespace PestanaDevApi.Constants
{
    public static partial class ErrorMessages
    {
        public const string EmailAlreadyBeingUsed = "The provided email is already in use.";
        public const string InvalidEmailFormat = "The provided email format is invalid.";
        public const string EmailAlreadySended = "The provided email was already sended";
        public const string EmailSendingError = "An error occurred when communicating with the email server!";
        public const string EmailUnespectedError = "An unespected error ocurred while sending the email!";
        public const string EmailDoestNotExists = "The provided email does not exists on our database!";
    }
}
