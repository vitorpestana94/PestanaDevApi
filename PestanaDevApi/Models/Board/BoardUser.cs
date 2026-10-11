namespace PestanaDevApi.Models.Board
{
    public class BoardUser
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid BoardId { get; set; } = Guid.Empty;

        public Guid UserId { get; set; } = Guid.Empty;

        public DateTime JoinedAt { get; set; }
    }
}
