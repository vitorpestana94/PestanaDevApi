using PestanaDevApi.Constants.Messages;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class ChangePasswordResponseDto: DefaultResponseDto
    {
        public ChangePasswordResponseDto() : base(SuccessMessages.UserPasswordChangeded)
        {
        }

        public ChangePasswordResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public ChangePasswordResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public ChangePasswordResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
