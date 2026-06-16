namespace PestanaDevApi.Constants
{
    public class GoogleConstants
    {
        public class GoogleApisHttpClient
        {
            public const string ErrorMessage = "error_message";
            public const string Result = "result";
            public const string Rating = "rating";
            public const string Name = "name";
            public const string TwoC = "%2C";
            public const string RecaptchaUrl = "https://www.google.com/recaptcha/api/siteverify?";

            public static string GetRecaptchaUrlParams(string captchaToken, string captchaSecretKey) => $"secret={captchaSecretKey}&response={captchaToken}";
            public static string GetPlacesSearchUrlParams(string placeId, string apiKey) => $"&place_id={placeId}&key={apiKey}";
        }

        public class ExternalAuth
        {
            public const string Google = "google";
            public const string Apple = "apple";
            public const string AppleOauth = "apple-oauth";
        }

        public class GoogleCaptchaErroCodes
        {
            public const string MissingInputSecret = "missing-input-secret";
            public const string InvalidInputSecret = "invalid-input-secret";
            public const string MissingInputResponse = "missing-input-response";
            public const string InvalidInputResponse = "invalid-input-response";
            public const string BadRequest = "bad-request";
            public const string TimeoutOrDuplicate = "timeout-or-duplicate";
        }
    }
}
