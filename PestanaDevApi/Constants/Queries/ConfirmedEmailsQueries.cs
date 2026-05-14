namespace PestanaDevApi.Constants.Queries
{
    public static class ConfirmedEmailsQueries
    {
        public const string Insert = @"
        INSERT INTO CONFIRMED_EMAILS(user_email)
        VALUES(@UserEmail);";

        public const string SelectExistEmail = @"
        SELECT EXISTS (
            SELECT 1
            FROM CONFIRMED_EMAILS
            WHERE user_email = @UserEmail
        );";
    }
}
