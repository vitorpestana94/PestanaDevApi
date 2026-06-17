namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string CaptchaV3_EmptyToken = "Captcha token was not provided.";
        public const string CaptchaV3_RequestFailed = "Captcha validation request failed.";
        public const string CaptchaV3_UserBehaviorIsNotHuman = "Google's score response indicates that user's behavior is not human.";
        public const string CaptchaV3_ResponseWithErrors = "Google answered with errors.";
        public const string CaptchaV3_MissingSecret = "Google's secret key is missing on request.";
        public const string CaptchaV3_InvalidSecret = "Google's secret key sended on request is invalid.";
        public const string CaptchaV3_MissingToken = "Google's captcha token is missing on request.";
        public const string CaptchaV3_InvalidToken = "Google's captcha token sended on request is invalid.";
        public const string CaptchaV3_BadRequest = "The request for google's captcha v3 API is invalid or malformed.";
        public const string CaptchaV3_TimeoutOrDuplicate = "The response from google's API it's not valid anymore: it's too old or it was already used.";
    }
}
