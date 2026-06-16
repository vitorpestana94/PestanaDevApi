using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Utils
{
    public interface IGoogleApisHttpClient
    {
        /// <summary>
        /// Makes a request for google's recaptcha v3 API
        /// </summary>
        /// <param name="captchaToken">O token a ser verificado</param>
        /// <returns>Um objeto</returns>
        Task<GetCaptchaV3ValidationResponse> GetCaptchaV3Validation(string captchaToken);
    }
}
