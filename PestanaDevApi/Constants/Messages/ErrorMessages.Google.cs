namespace PestanaDevApi.Constants.Messages
{
    public static partial class ErrorMessages
    {
        public const string CaptchaV3_EmptyToken = "A19F : Captcha token was not provided.";
        public const string CaptchaV3_RequestFailed = "94HB : Captcha validation request failed.";
        public const string CaptchaV3_UserBehaviorIsNotHuman = "BDC2 : Google's score response indicates that user's behavior is not human.";
        public const string CaptchaV3_ResponseWithErrors = "AF0C : Google answered with errors.";
        public const string CaptchaV3_MissingSecret = "A02D : Google's secret key is missing on request.";
        public const string CaptchaV3_InvalidSecret = "4C64 : Google's secret key sended on request is invalid.";
        public const string CaptchaV3_MissingToken = "B3F9 : Google's captcha token is missing on request.";
        public const string CaptchaV3_InvalidToken = "C7AE : Google's captcha token sended on request is invalid.";
        public const string CaptchaV3_BadRequest = "4JF2 : The request for google's captcha v3 API is invalid or malformed.";
        public const string CaptchaV3_TimeoutOrDuplicate = "BKC4 : The response from google's API it's not valid anymore: it's too old or it was already used.";
    }
}
