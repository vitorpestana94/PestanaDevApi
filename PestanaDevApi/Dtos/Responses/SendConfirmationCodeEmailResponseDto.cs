using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class SendConfirmationCodeEmailResponseDto: DefaultResponse
    {

        public SendConfirmationCodeEmailResponseDto() : base()
        {
        }

        public SendConfirmationCodeEmailResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
