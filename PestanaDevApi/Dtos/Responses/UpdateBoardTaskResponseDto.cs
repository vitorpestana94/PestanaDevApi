using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class UpdateBoardTaskResponseDto: DefaultResponseDto
    {
        public UpdateBoardTaskResponseDto() : base(SuccessMessages.TaskUpdated)
        {
        }

        public UpdateBoardTaskResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public UpdateBoardTaskResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public UpdateBoardTaskResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
