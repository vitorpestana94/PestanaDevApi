using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class DeleteBoardResponseDto: DefaultResponseDto
    {
        public DeleteBoardResponseDto() : base(SuccessMessages.BoardDeleted)
        {
        }

        public DeleteBoardResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public DeleteBoardResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public DeleteBoardResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
