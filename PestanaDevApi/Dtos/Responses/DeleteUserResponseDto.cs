using PestanaDevApi.Constants.Messages;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class DeleteUserResponseDto: DefaultResponseDto
    {
        public DeleteUserResponseDto() : base(SuccessMessages.UserDataUpdated)
        {
        }

        public DeleteUserResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public DeleteUserResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public DeleteUserResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
