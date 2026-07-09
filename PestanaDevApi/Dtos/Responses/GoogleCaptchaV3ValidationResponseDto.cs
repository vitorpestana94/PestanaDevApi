using Consts = PestanaDevApi.Constants.GoogleConstants.GoogleCaptchaErroCodes;
using ErrorMessage = PestanaDevApi.Constants.Messages.ErrorMessages;
using PestanaDevApi.Exceptions;
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
        public IEnumerable<string> ErrorsCodes { get; set; } = [];

        public bool IsHumanBehavior => Score >= 0.5;
        private bool IsResponseWithErrors => ErrorsCodes?.Any() ?? false;

        public void ThrowIfResponseHasErrors()
        {

            if (IsResponseWithErrors)
            {
                string error = ErrorsCodes?.FirstOrDefault() ?? string.Empty;
                // It may happen that two or more erros returns from google's API on the same request. In that case, it was decided that the first one will be thrown.
                throw error switch
                {  // All those errors below are documentated here https://developers.google.com/recaptcha/docs/verify?hl=pt-br
                    (Consts.MissingInputSecret) => new ApiException(ErrorMessage.CaptchaV3_MissingSecret, 500),
                    (Consts.InvalidInputSecret) => new ApiException(ErrorMessage.CaptchaV3_InvalidSecret, 500),
                    (Consts.MissingInputResponse) => new ApiException(ErrorMessage.CaptchaV3_MissingToken, 500),
                    (Consts.InvalidInputResponse) => new ApiException(ErrorMessage.CaptchaV3_InvalidToken, 500),
                    (Consts.BadRequest) => new ApiException(ErrorMessage.CaptchaV3_BadRequest, 500),
                    (Consts.TimeoutOrDuplicate) => new ApiException(ErrorMessage.CaptchaV3_TimeoutOrDuplicate, 500),
                    _ => new ApiException(ErrorMessage.CaptchaV3_ResponseWithErrors, 500),
                };
            }
        }
    }
}
