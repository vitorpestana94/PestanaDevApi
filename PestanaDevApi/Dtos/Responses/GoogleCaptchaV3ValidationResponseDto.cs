/**
 * Copyright (c) 2026, Prion Corporation. All rights reserved
*/
using Consts = PestanaDevApi.Constants.GoogleConstants.GoogleCaptchaErroCodes;
using System.Text.Json.Serialization;

namespace PestanaDevApi.Dtos.Responses
{
    public class GoogleCaptchaV3ValidationResponseDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("hostname")]
        public string HostName { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("challenge_ts")]
        public string ChallengeTs { get; set; } = string.Empty;

        [JsonPropertyName("error-codes")]
        public IEnumerable<string> ErrorsCodes { get; set; } = Enumerable.Empty<string>();

        public bool IsHumanBehavior => Score >= 0.5;
        private bool IsResponseWithErrors => ErrorsCodes?.Any() ?? false;

        public void ThrowIfResponseHasErrors()
        {

            if (IsResponseWithErrors)
            {
                //string error = ErrorsCodes?.FirstOrDefault() ?? string.Empty;
                ////  It may happen that two or more erros returns from google's API on the same request. In that case, it was decided that the first one will be thrown.
                //throw error switch
                //{  // All those errors below are documentated here https://developers.google.com/recaptcha/docs/verify?hl=pt-br
                //    (Consts.MissingInputSecret) => new ApiException(ApiExceptionMessage.ApiError.CaptchaV3_MissingSecret, false),
                //    (Consts.InvalidInputSecret) => new ApiException(ApiExceptionMessage.ApiError.CaptchaV3_InvalidSecret, false),
                //    (Consts.MissingInputResponse) => new ApiException(ApiExceptionMessage.ApiError.CaptchaV3_MissingToken, false),
                //    (Consts.InvalidInputResponse) => new ApiException(ApiExceptionMessage.ApiError.CaptchaV3_InvalidToken, false),
                //    (Consts.BadRequest) => new ApiException(ApiExceptionMessage.ApiError.CaptchaV3_BadRequest, false),
                //    (Consts.TimeoutOrDuplicate) => new ApiException(ApiExceptionMessage.ApiError.CaptchaV3_TimeoutOrDuplicate, false),
                //    _ => new ApiException(ApiExceptionMessage.ApiError.CaptchaV3_ResponseWithErrors, false),
                //};
            }
        }
    }
}
