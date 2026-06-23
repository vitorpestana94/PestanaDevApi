namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string BadRequest = $"Your request does not meet the requirements.";
        public const string Unauthorized = $" You do not have permission to access this resource!";
        public const string DefaultMessage = "Something went wrong!";
        public const string NotFound = $"the resource was not found.";
        public const string UnsupportedMediaType = "You requested with a wrong type of media!";
        public const string InternalServerError = $"An unexpected error occurred.";
    }
}
