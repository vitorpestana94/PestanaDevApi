namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string InvalidCredentials = "Invalid Credentials.";
        public const string InvalidLoginEndpointUserWithPlatform = "Invalid Login Endpoint: you registered using a platform. Please, login using platform.";
        public const string InvalidLoginEndpointUserWithPassword = "Invalid Login Endpoint: you registered manually. Please, login using your email and password.";
        public const string UserNotFound = $"{NotFoundTitle} user not found!";
        public const string UserBeheaviorItsNotHuman = $"{ForbiddenTitle} Google's score response indicates that user's behavior is not human.";
        #region User
        public const string UserNotUpdated = $"{InternalServerErrorTitle} user was not updated due internal errors!";
        public const string UserNotDeleted = $"{InternalServerErrorTitle} user was not deleted due internal errors!";
        public const string RequestDontHaveAnyChangedData = $"{BadRequestTitle} the request dont have any changeded data!";
        public const string UserPasswordNotUpdated = $"{InternalServerErrorTitle} user password was not deleted due internal errors!";
        public const string UserDontHavePassword = $"{BadRequestTitle} can't update user password!";
        #endregion
    }
}
