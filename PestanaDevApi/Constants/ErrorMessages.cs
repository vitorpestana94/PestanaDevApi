namespace PestanaDevApi.Constants
{
    public static class ErrorMessages
    {
        public const string InvalidCredentials = "Invalid Credentials.";
        public const string BadRequest = "Bad Request: Your request does not meet the requirements.";
        public const string Unauthorized = "Unauthorised access: You do not have permission to access this resource!";
        public const string DefaultMessage = "Something went wrong!";

        #region Email
        public const string EmailAlreadyBeingUsed = "The provided email is already in use.";
        public const string InvalidEmailFormat = "The provided email format is invalid.";
        public const string EmailSmtp = "Email smtp not configured!";
        public const string EmailAddress = "Email address not configured!";
        public const string EmailAppPassword = "Email app-password not configured!";
        public const string EmailSendingError = "An error occurred when communicating with the email server!";
        public const string EmailUnespectedError = "An unespected error ocurred while sending the email!";
        #endregion
    }
}
