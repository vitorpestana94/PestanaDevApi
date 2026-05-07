using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class SendConfirmationCodeEmailResponseDto: DefaultResponseDto
    {

        public SendConfirmationCodeEmailResponseDto() : base()
        {
        }

        public SendConfirmationCodeEmailResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
