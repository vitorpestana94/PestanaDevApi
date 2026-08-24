using System.Text;
using System.Net;
using PestanaDevApi.Interfaces.Utils;
using PestanaDevApi.Dtos.Responses;
using Consts = PestanaDevApi.Constants.GoogleConstants.GoogleApisHttpClient;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Utils
{
    public class GoogleApisHttpClient : IGoogleApisHttpClient
    {
        private readonly IConfiguration _config;
        private readonly IRequestService _http;

        public GoogleApisHttpClient(IConfiguration config, IRequestService http)
        {
            _config = config;
            _http = http;
        }

        /// <summary>
        /// Request google's captcha validation API
        /// </summary>
        /// <param name="captchaToken">Google's captcha token</param>
        /// <returns>Object: google's captcha v3 validation response.</returns>
        public async Task<GetCaptchaV3ValidationResponse> GetCaptchaV3Validation(string captchaToken)
        {
            string? url = CreateCaptchaRequesthUrl(captchaToken);

            if (string.IsNullOrEmpty(url))
                return new GetCaptchaV3ValidationResponse(HttpStatusCode.InternalServerError);

            GoogleCaptchaV3ValidationResponseDto? response;

            try
            {
                response = await _http.RequestAsync<GoogleCaptchaV3ValidationResponseDto>(url);
            }
            catch
            {
                return new GetCaptchaV3ValidationResponse(HttpStatusCode.InternalServerError);
            }

            if (response == null)
                return new GetCaptchaV3ValidationResponse(HttpStatusCode.InternalServerError);

            return new GetCaptchaV3ValidationResponse(response);
        }

        #region Private Methods
        /// <summary>
        /// Builds the Google captcha verification API URL for retrieving .
        /// </summary>
        /// <param name="captchaToken">The token provided by google's captcha v3</param>
        /// <param name="captchaSecretKey">The google's captcha secret key</param>
        /// <returns>A fully constructed URL for the Google Places API details request.</returns>
        private string? CreateCaptchaRequesthUrl(string captchaToken)
        {
            string captchaSecretKey = _config["captchaSecret"] ?? "";

            if (string.IsNullOrEmpty(captchaSecretKey) || string.IsNullOrEmpty(captchaToken))
                return null;

            return GetRecaptchaUrl(captchaToken, captchaSecretKey);
        }


        private static string GetRecaptchaUrl(string captchaToken, string captchaSecretKey)
        {
            StringBuilder url = new();

            url.Append(Consts.RecaptchaUrl);
            url.Append(Consts.GetRecaptchaUrlParams(captchaToken, captchaSecretKey));

            return url.ToString();
        }
        #endregion
    }
}
