using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class CreateBoardResponseDto: DefaultResponseDto
    {
        public CreateBoardResponseDto() : base()
        {
        }

        public CreateBoardResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
