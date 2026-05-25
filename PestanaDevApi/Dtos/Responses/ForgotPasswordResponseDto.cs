using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class ForgotPasswordResponseDto: DefaultResponseDto
    {
        public ForgotPasswordResponseDto() : base()
        {
        }

        public ForgotPasswordResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }

        public ForgotPasswordResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
