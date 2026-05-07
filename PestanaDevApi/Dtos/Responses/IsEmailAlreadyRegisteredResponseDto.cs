using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class IsEmailAlreadyRegisteredResponseDto: DefaultResponseDto
    {
        public IsEmailRegistered? IsRegistered { get; set; }

        public IsEmailAlreadyRegisteredResponseDto()
        {
        }

        public IsEmailAlreadyRegisteredResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public IsEmailAlreadyRegisteredResponseDto(bool isRegistered) : base()
        {
            IsRegistered = new(isRegistered);
        }

        public IsEmailAlreadyRegisteredResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public class IsEmailRegistered
        {
            public bool IsRegistered { get; set; }

            public IsEmailRegistered() { }
            public IsEmailRegistered(bool isRegistered)
            {
                IsRegistered = isRegistered;
            }
        }
    }
}
