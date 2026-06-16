namespace PestanaDevApi.Constants.Queries
{
    public static class UserQueries
    {
        public const string GetUser = @"
        SELECT
            user_name, 
            user_email,
            user_password,
            signup_by_platform 
        FROM
            USERS_PROFILE_DATA
        WHERE
            id = @UserId;";

        public const string UpdateUser = @"
        UPDATE
            USERS_PROFILE_DATA
        SET
            user_name = @Name,
            user_email = @Email
        WHERE
            id = @UserId;";

        public const string DeleteUser = @"
        DELETE FROM 
            USERS_PROFILE_DATA
        WHERE
            id = @UserId;
        
        DELETE FROM
            USERS_PROFILE_PLATFORM_DATA
        WHERE
            user_id = @UserId;";

        public const string UpdateUserPassword = @"
        UPDATE
            USERS_PROFILE_DATA
        SET
            user_password = @Password
        WHERE
            id = @UserId;";
    }
}
