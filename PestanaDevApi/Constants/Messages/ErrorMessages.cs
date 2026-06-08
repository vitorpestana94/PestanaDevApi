namespace PestanaDevApi.Constants
{
    public static partial class ErrorMessages
    {
        public const string InvalidCredentials = "Invalid Credentials.";
        public const string InvalidLoginEndpoint = "Invalid Login Endpoint: you registered using a platform. Please, login using platform.";

        public const string UserNotFound = "Not Found: user not found!";
        public const string UserNotUpdated = "Internal Server Error: user was not updated due internal errors!";
        public const string UserNotDeleted = "Internal Server Error: user was not deleted due internal errors!";
        public const string RequestDontHaveAnyChangedData = "Bad Request: the request dont have any changeded data!";
    }
}
