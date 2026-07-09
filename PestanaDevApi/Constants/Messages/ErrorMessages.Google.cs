namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string CaptchaV3_EmptyToken = "A19F : Captcha token was not provided.";
        public const string CaptchaV3_RequestFailed = "A19F : Captcha validation request failed.";
        public const string CaptchaV3_UserBehaviorIsNotHuman = "A19F : Google's score response indicates that user's behavior is not human.";
        public const string CaptchaV3_ResponseWithErrors = "A19F : Google answered with errors.";
        public const string CaptchaV3_MissingSecret = "A19F : Google's secret key is missing on request.";
        public const string CaptchaV3_InvalidSecret = "A19F : Google's secret key sended on request is invalid.";
        public const string CaptchaV3_MissingToken = "A19F : Google's captcha token is missing on request.";
        public const string CaptchaV3_InvalidToken = "A19F : Google's captcha token sended on request is invalid.";
        public const string CaptchaV3_BadRequest = "A19F : The request for google's captcha v3 API is invalid or malformed.";
        public const string CaptchaV3_TimeoutOrDuplicate = "A19F : The response from google's API it's not valid anymore: it's too old or it was already used.";
    }
}
