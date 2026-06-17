using PestanaDevApi.Models;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class AuthResponseDto: DefaultResponseDto
    {
        public ApiToken? ApiTokens { get; set; } = null;

        public AuthResponseDto()
        {
        }

        public AuthResponseDto(HttpStatusCode statusCode) : base(statusCode)
        { 
        }

        public AuthResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }

        public AuthResponseDto(ApiToken apiToken) : base() 
        {
            ApiTokens = apiToken;
        }

        public AuthResponseDto(string errorMessage) : base(HttpStatusCode.Unauthorized, errorMessage)
        {
        }
    }
}
