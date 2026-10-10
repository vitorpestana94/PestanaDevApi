using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class UpdateBoardColumnResponseDto: DefaultResponseDto
    {
        public UpdateBoardColumnResponseDto() : base(SuccessMessages.ColumnUpdated)
        {
        }

        public UpdateBoardColumnResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public UpdateBoardColumnResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public UpdateBoardColumnResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
