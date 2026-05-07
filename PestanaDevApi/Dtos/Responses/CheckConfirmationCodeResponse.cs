using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class CheckConfirmationCodeResponse: DefaultResponseDto
    {
        public CheckConfirmationCodeResponse() : base()
        {
        }

        public CheckConfirmationCodeResponse(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
