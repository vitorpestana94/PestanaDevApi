using System.Net;
using PestanaDevApi.Models;

namespace PestanaDevApi.Dtos.Responses
{
    /// <summary>
    /// Specialized response for platform-based registration.
    /// Inherits from <see cref="AuthResponseDto"/> to provide a semantic distinction
    /// while maintaining the same authentication structure.
    /// </summary>
    public class SignUpWithPlatformResponseDto : AuthResponseDto
    {
        public SignUpWithPlatformResponseDto() : base()
        {
        }

        public SignUpWithPlatformResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public SignUpWithPlatformResponseDto(AuthResponseDto response)
        {
            IsSuccess = response.IsSuccess;
            StatusCode = response.StatusCode;
            Message = response.Message;
            ApiTokens = response.ApiTokens;
        }

        public SignUpWithPlatformResponseDto(string errorMessage) : base(errorMessage)
        {
        }
    }
}