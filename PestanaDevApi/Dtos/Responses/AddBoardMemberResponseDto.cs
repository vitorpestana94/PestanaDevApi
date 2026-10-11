using System.Net;
using PestanaDevApi.Constants.Messages;

namespace PestanaDevApi.Dtos.Responses
{
    public class AddBoardMemberResponseDto: DefaultResponseDto
    {
        /// <summary>
        /// True when the user wasn't registered, so an invitation to sign up was sent by email instead.
        /// </summary>
        public bool InvitationSent { get; set; }

        public AddBoardMemberResponseDto() : base(SuccessMessages.MemberAdded)
        {
        }

        public AddBoardMemberResponseDto(bool invitationSent) : base(invitationSent ? SuccessMessages.MemberInvited : SuccessMessages.MemberAdded)
        {
            InvitationSent = invitationSent;
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
