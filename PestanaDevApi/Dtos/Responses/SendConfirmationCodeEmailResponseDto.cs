using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class SendConfirmationCodeEmailResponseDto: DefaultResponse
    {
        public string ResendCodeToken { get; set; } = string.Empty;

        public SendConfirmationCodeEmailResponseDto() : base()
        {
        }

        public SendConfirmationCodeEmailResponseDto(string resentJwt) : base()
        {
            ResendCodeToken = resentJwt;
        }

        public SendConfirmationCodeEmailResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
