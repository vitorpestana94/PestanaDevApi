using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class DeleteBoardColumnResponseDto: DefaultResponseDto
    {
        public DeleteBoardColumnResponseDto() : base(SuccessMessages.ColumnDeleted)
        {
        }

        public DeleteBoardColumnResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public DeleteBoardColumnResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public DeleteBoardColumnResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
