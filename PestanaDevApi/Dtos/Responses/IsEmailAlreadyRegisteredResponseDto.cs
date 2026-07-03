using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class IsEmailAlreadyRegisteredResponseDto: DefaultResponseDto
    {
        public bool IsRegistered { get; set; }

        public IsEmailAlreadyRegisteredResponseDto()
        {
        }

        public IsEmailAlreadyRegisteredResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public IsEmailAlreadyRegisteredResponseDto(bool isRegistered) : base()
        {
            IsRegistered = isRegistered;
        }

        public IsEmailAlreadyRegisteredResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
