using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class DeleteBoardTaskResponseDto: DefaultResponseDto
    {
        public DeleteBoardTaskResponseDto() : base(SuccessMessages.TaskDeleted)
        {
        }

        public DeleteBoardTaskResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public DeleteBoardTaskResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public DeleteBoardTaskResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
