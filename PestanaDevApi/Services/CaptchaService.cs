using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Interfaces.Utils;

namespace PestanaDevApi.Services
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IGoogleApisHttpClient _googleApisHttpClient;

        public CaptchaService(IGoogleApisHttpClient googleApisHttpClient)
        {
            _googleApisHttpClient = googleApisHttpClient;
        }

        /// <summary>
        /// Request google's captcha v3 API.
        /// On google's response there's a json property named score that is a scale from 0.0 to 1
        /// that represents if user's behavior is human. If it's lower than 0.5, so it's not a human behavior
        /// and this method will return false.
        /// </summary>
        /// <param name="captchaToken">Google's captcha token</param>
        /// <returns>Bool that represents if user's behavior is human.</returns>
        public async Task<bool> ValidateCaptchaV3(string captchaToken)
        {
            GetCaptchaV3ValidationResponse response = await _googleApisHttpClient.GetCaptchaV3Validation(captchaToken);

            if (response.GoogleResponse == null)
                return false;

            response.GoogleResponse.ThrowIfResponseHasErrors();

            return response.GoogleResponse.IsHumanBehavior;
        }
    }
}
