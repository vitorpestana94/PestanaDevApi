using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class EmailResponseDto : DefaultResponseDto
    {
        public EmailResponseDto() : base()
        {
        }

        public EmailResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
