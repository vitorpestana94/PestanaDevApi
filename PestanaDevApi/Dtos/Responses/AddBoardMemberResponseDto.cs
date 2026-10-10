using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class AddBoardMemberResponseDto: DefaultResponseDto
    {
        public AddBoardMemberResponseDto() : base(SuccessMessages.MemberAdded)
        {
        }

        public AddBoardMemberResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public AddBoardMemberResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public AddBoardMemberResponseDto(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
