using System.Net;
using PestanaDevApi.Models;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Dtos.Responses
{
    public class GetBoardMembersResponseDto: DefaultResponseDto
    {
        public IEnumerable<BoardMember> Members { get; set; } = [];

        public GetBoardMembersResponseDto(IEnumerable<User> members, Board board) : base()
        {
            Members = members.Select(member => new BoardMember(member, isOwner: member.Id == board.OwnerId));
        }

        public GetBoardMembersResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }

    public class BoardMember
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsOwner { get; set; }

        public BoardMember() { }

        public BoardMember(User user, bool isOwner)
        {
            Id = user.Id;
            Name = user.UserName;
            Email = user.UserEmail;
            IsOwner = isOwner;
        }
    }
}
