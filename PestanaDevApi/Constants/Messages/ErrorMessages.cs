namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string InvalidCredentials = $"Invalid Credentials.";
        public const string InvalidLoginEndpointUserWithPlatform = $"you registered using a platform. Please, login using platform.";
        public const string InvalidLoginEndpointUserWithPassword = $"You registered manually. Please, login using your email and password.";
        public const string InvalidPlatformToken = $"The provided platform token is invalid!.";
        public const string UserNotFound = $"User was not found!";
        public const string UserBeheaviorItsNotHuman = $" Google's score response indicates that user's behavior is not human.";
        #region User
        public const string UserNotUpdated = $"User was not updated due internal errors!";
        public const string UserNotDeleted = $"User was not deleted due internal errors!";
        public const string RequestDontHaveAnyChangedData = $"Cannot update user data because no changes were detected.";
        public const string UserPasswordNotUpdated = $"User password was not deleted due internal errors!";
        public const string UserDontHavePassword = $"Can't update user password!";
        #endregion
    }
}
