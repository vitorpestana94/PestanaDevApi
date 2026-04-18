namespace PestanaDevApi.Constants.Queries
{
    public static class SignUpQueries
    {
        public const string InsertUser = @"
        INSERT INTO USERS_PROFILE_DATA (user_name, user_email, user_password)
        VALUES (@Name, @Email, @Password)
        RETURNING id;";

        public const string InsertUserByPlatform = @"
        INSERT INTO USERS_PROFILE_DATA (user_name, user_email, user_password)
        VALUES (@Name, @Email, @Password)
        RETURNING id;";

        public const string InsertUserPlatformData = @"
        INSERT INTO USERS_PROFILE_PLATFORM_DATA (user_id, user_signup_platform, user_platform_id)
        VALUES (@UserId, @Platform, @PlatformId);";

        public const string SelectUserIdByEmail = @"
        SELECT 
            id
        FROM
            USERS_PROFILE_DATA
        WHERE
            user_email = @Email;";
    }
}
