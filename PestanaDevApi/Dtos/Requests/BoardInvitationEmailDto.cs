namespace PestanaDevApi.Dtos.Requests
{
    /// <summary>
    /// Data of the email that invites a not registered user to sign up and join a board.
    /// </summary>
    public class BoardInvitationEmailDto: EmailRequest
    {
        public Guid BoardId { get; set; } = Guid.Empty;
        public string OwnerEmail { get; set; } = string.Empty;

        public BoardInvitationEmailDto() { }

        public BoardInvitationEmailDto(Guid boardId, string invitedEmail, string ownerEmail, string clientLocale)
        {
            BoardId = boardId;
            ClientEmail = invitedEmail;
            OwnerEmail = ownerEmail;
            ClientLocale = clientLocale;
        }
    }
}
