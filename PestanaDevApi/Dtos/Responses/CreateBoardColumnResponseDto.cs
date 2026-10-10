using System.Net;
using PestanaDevApi.Constants.Messages;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Dtos.Responses
{
    public class CreateBoardColumnResponseDto: DefaultResponseDto
    {
        public BoardColumn? Column { get; set; } = null;

        public CreateBoardColumnResponseDto(BoardColumn column) : base(SuccessMessages.ColumnCreated)
        {
            Column = column;
        }

        public CreateBoardColumnResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public CreateBoardColumnResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public CreateBoardColumnResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
