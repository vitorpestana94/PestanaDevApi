using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class CheckIfConfirmationCodeEmailAlreadySentResponseDto: DefaultResponseDto
    {
        public bool ConfirmationCodeAlreadySent { get; set; }

        public CheckIfConfirmationCodeEmailAlreadySentResponseDto(bool confirmationCodeAlreadySent) : base()
        {
            ConfirmationCodeAlreadySent = confirmationCodeAlreadySent;
        }

        public CheckIfConfirmationCodeEmailAlreadySentResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
