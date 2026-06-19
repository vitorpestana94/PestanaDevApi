namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string BadRequest = $"{BadRequestTitle} Your request does not meet the requirements.";
        public const string Unauthorized = $"{UnauthorizedTitle}: You do not have permission to access this resource!";
        public const string DefaultMessage = "Something went wrong!";
        public const string NotFound = $"{NotFoundTitle} the resource was not found.";
        public const string UnsupportedMediaType = "Unsupported Media Type: you requested with a wrong type of media!";
        public const string InternalServerError = $"{InternalServerErrorTitle} An unexpected error occurred.";
    }
}
