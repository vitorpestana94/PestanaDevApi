using PestanaDevApi.Models;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class SignUpResponseDto : DefaultResponse
    {
        public ApiToken ApiTokens { get; set; } = new();

        public SignUpResponseDto() { }

        public SignUpResponseDto(ApiToken apiToken) : base()
        {
            ApiTokens = apiToken;
        }

        public SignUpResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public SignUpResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }
    }
}
