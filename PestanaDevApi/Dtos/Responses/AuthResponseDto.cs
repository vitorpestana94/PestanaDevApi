using PestanaDevApi.Models;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class AuthResponseDto: DefaultResponse
    {
        public ApiToken ApiTokens { get; set; } = new();

        public AuthResponseDto()
        {
        }

        public AuthResponseDto(HttpStatusCode statusCode) : base(statusCode)
        { 
        }
        
        public AuthResponseDto(ApiToken apiToken) : base() 
        {
            ApiTokens = apiToken;
        }

        public AuthResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
