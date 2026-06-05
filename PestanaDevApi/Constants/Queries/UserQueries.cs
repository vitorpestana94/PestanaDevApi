namespace PestanaDevApi.Constants.Queries
{
    public static class UserQueries
    {
        public const string GetUser = @"
        SELECT
            user_name, 
            user_email, 
            signup_by_platform 
        FROM
            USERS_PROFILE_DATA
        WHERE
            id = @UserId";
    }
}
