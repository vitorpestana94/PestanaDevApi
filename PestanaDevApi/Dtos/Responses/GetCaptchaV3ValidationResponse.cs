using PestanaDevApi.Models;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class GetCaptchaV3ValidationResponse: DefaultResponseDto
    {

        public GoogleCaptchaV3ValidationResponseDto? GoogleResponse { get; set; }

        public GetCaptchaV3ValidationResponse()
        {
            GoogleResponse = null;
        }

        public GetCaptchaV3ValidationResponse(HttpStatusCode statusCode) : base(statusCode)
        {
            GoogleResponse = null;
        }

        public GetCaptchaV3ValidationResponse(GoogleCaptchaV3ValidationResponseDto googleResponse) : base()
        {
            GoogleResponse = googleResponse;
        }

        public GetCaptchaV3ValidationResponse(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
            GoogleResponse = null;
        }
    }
}
