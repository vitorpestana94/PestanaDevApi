namespace PestanaDevApi.Constants.Queries
{
    public static class ForgotPasswordQueries
    {
        public const string UpdateUserPassword = @"
        UPDATE
            USERS_PROFILE_DATA
        SET
            user_password = @Password
        WHERE
            user_email = @Email;";
    }
}
