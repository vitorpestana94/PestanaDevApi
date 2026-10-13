using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class HubTokenResponseDto: DefaultResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public HubTokenResponseDto(string token) : base()
        {
            Token = token;
        }

        public HubTokenResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
