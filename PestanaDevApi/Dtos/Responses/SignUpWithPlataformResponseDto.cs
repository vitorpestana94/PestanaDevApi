using System.Net;
using PestanaDevApi.Models;

namespace PestanaDevApi.Dtos.Responses
{
    /// <summary>
    /// Specialized response for platform-based registration.
    /// Inherits from <see cref="LoginResponseDto"/> to provide a semantic distinction
    /// while maintaining the same authentication structure.
    /// </summary>
    public class SignUpWithPlatformResponseDto : LoginResponseDto
    {
        public SignUpWithPlatformResponseDto() : base()
        {
        }

        public SignUpWithPlatformResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public SignUpWithPlatformResponseDto(LoginResponseDto response)
        {
            IsSuccess = response.IsSuccess;
            StatusCode = response.StatusCode;
            ErrorMessage = response.ErrorMessage;
            ApiTokens = response.ApiTokens;
        }

        public SignUpWithPlatformResponseDto(string errorMessage) : base(errorMessage)
        {
        }
    }
}