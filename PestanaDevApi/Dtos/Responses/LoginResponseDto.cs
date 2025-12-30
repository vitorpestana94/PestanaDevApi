using PestanaDevApi.Models;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class LoginResponseDto: DefaultResponse
    {
        public ApiToken ApiTokens { get; set; } = new();

        public LoginResponseDto()
        {
        }

        public LoginResponseDto(HttpStatusCode statusCode) : base(statusCode)
        { 
        }
        
        public LoginResponseDto(ApiToken apiToken) : base() 
        {
            ApiTokens = apiToken;
        }

        public LoginResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
