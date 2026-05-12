namespace PestanaDevApi.Constants.Queries
{
    public static class TokenQueries
    {
        public const string DeleteExpiredRefreshTokens = @"
        DELETE FROM 
            REFRESH_TOKEN 
        WHERE 
            UTC_TIMESTAMP() >= DATE_ADD(expired_at, INTERVAL 2 DAY);";

        public const string UpdateRrefreshToken = @"
        UPDATE 
            REFRESH_TOKEN 
        SET    
            token = @Token, 
            expired_at = @ExpiredAt 
        WHERE 
            user_profile_id = @userId 
        AND 
            device_id = @deviceId;";

        public const string InserRefreshToken = @"
        INSERT INTO REFRESH_TOKEN (user_profile_id, device_id, token, expired_at) 
        VALUES (@UserId, @DeviceId, @Token, @ExpiredAt);";

        public const string SelectRefreshToken = @"
        SELECT 
            id 
        FROM 
           REFRESH_TOKEN 
        WHERE 
            user_profile_id = @UserId 
        AND 
            device_id = @DeviceId;";
    }
}
