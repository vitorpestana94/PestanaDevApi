namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string InvalidCredentials = $"4A7F : Invalid Credentials.";
        public const string InvalidLoginEndpointUserWithPlatform = $"9D21 : You registered using a platform. Please, login using platform.";
        public const string InvalidLoginEndpointUserWithPassword = $"C5E8 : You registered manually. Please, login using your email and password.";
        public const string InvalidPlatformToken = $"E31B : The provided platform token is invalid!.";
        public const string UserBeheaviorItsNotHuman = $"7F9C : Google's score response indicates that user's behavior is not human.";
        public const string ErrorDuringRefreshTokenCreation = "A2D6 : An unexpected error ocurred while trying to create the refresh token.";

        #region User
        public const string UserNotFound = $"5BC1 : User was not found!";
        public const string UserNotUpdated = $"8E4D : User was not updated due internal errors!";
        public const string UserNotDeleted = $"D913 : User was not deleted due internal errors!";
        public const string RequestDontHaveAnyChangedData = $"F26A : Cannot update user data because no changes were detected.";
        public const string UserPasswordNotUpdated = $"1C7E : User password was not deleted due internal errors!";
        public const string UserDontHavePassword = $"B4F0 : Can't update user password!";
        #endregion
    }
}
