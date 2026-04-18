namespace PestanaDevApi.Constants.Queries
{
    public static class ConfirmationCodeQueries
    {
        public const string InserConfirmationCode = @"
        INSERT INTO CONFIRMATION_CODE(user_email, confirmation_code)
        VALUES(@UserEmail, @ConfirmationCode);";

        public const string SelectOneIfTheresEmail = @"
        SELECT
            1
        FROM
            CONFIRMATION_CODE
        WHERE
            user_email = @UserEmail;";

        public const string SelectOneIfCodeIsStillFresh = @"
        SELECT 
	        1
        FROM 
	        CONFIRMATION_CODE
        WHERE
	        user_email = @UserEmail
        AND
	        UTC_TIMESTAMP() <= DATE_ADD(created_at, INTERVAL 10 MINUTE);";

        public const string SelectCodeByEmail = @"
        SELECT 
            confirmation_code
        FROM
           CONFIRMATION_CODE 
        WHERE
            user_email = @UserEmail;";

        public const string DeleteConfirmationCode = @"
        DELETE FROM CONFIRMATION_CODE
        WHERE
	        user_email = @UserEmail;";
    }
}
