using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class RemoveBoardMemberResponseDto: DefaultResponseDto
    {
        public RemoveBoardMemberResponseDto() : base(SuccessMessages.MemberRemoved)
        {
        }

        public RemoveBoardMemberResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public RemoveBoardMemberResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public RemoveBoardMemberResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
