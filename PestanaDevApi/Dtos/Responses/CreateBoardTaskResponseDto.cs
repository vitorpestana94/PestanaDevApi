using System.Net;
using PestanaDevApi.Constants.Messages;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Dtos.Responses
{
    public class CreateBoardTaskResponseDto: DefaultResponseDto
    {
        public BoardTask? Task { get; set; } = null;

        public CreateBoardTaskResponseDto(BoardTask task) : base(SuccessMessages.TaskCreated)
        {
            Task = task;
        }

        public CreateBoardTaskResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public CreateBoardTaskResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public CreateBoardTaskResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
