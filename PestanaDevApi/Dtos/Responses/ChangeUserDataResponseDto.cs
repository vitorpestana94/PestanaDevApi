using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class ChangeUserDataResponseDto: DefaultResponseDto
    {
        public ChangeUserDataResponseDto() : base(SuccessMessages.UserDataUpdated)
        {
        }

        public ChangeUserDataResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public ChangeUserDataResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public ChangeUserDataResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
