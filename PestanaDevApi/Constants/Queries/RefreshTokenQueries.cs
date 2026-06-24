namespace PestanaDevApi.Constants.Queries
{
    public static class RefreshTokenQueries
    {

        public const string SelectUserDataByRefreshToken = @"
        SELECT 
            u.id
        FROM 
            REFRESH_TOKEN rt
        JOIN 
            USERS_PROFILE_DATA u 
        ON 
            u.id = rt.user_profile_id
        WHERE 
            rt.user_profile_id = @UserId 
        AND 
            rt.token = @Token 
        AND 
            rt.device_id = @DeviceId 
        AND 
            expired_at > NOW();";
    }
}
