namespace PestanaDevApi.Constants.Queries
{
    public static class LoginQueries
    {
        public const string SelectUserDataByEmail = @"
        SELECT
            id, user_name, user_email, user_password, signup_by_platform 
        FROM
            USERS_PROFILE_DATA
        WHERE
            user_email = @Email;";

        public const string SelectUserIdByEmail = @"
        SELECT
            id 
        FROM
            USERS_PROFILE_DATA
        WHERE
            user_email = @Email;";

        public const string SelectUserIdByPlatformId = @"
        SELECT
            user_id
        FROM
            USERS_PROFILE_PLATFORM_DATA
        WHERE
            user_signup_platform = @Platform
        AND   
            user_platform_id = @PId;";

        public const string DeleteRefreshToken = @"
        DELETE FROM REFRESH_TOKEN
        WHERE 
            user_profile_id = @UserId
        AND
            device_id = @DeviceId;";
    }
}
