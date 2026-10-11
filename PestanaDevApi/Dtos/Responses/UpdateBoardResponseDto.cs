using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class UpdateBoardResponseDto: DefaultResponseDto
    {
        public UpdateBoardResponseDto() : base(SuccessMessages.BoardUpdated)
        {
        }

        public UpdateBoardResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public UpdateBoardResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public UpdateBoardResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
