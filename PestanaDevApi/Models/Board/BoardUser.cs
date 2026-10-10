namespace PestanaDevApi.Models.Board
{
    public class BoardUser
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid BoardId { get; set; } = Guid.Empty;

        public Guid UserId { get; set; } = Guid.Empty;

        public DateTime JoinedAt { get; set; }

        public Board Board { get; set; } = null!;

        public User User { get; set; } = null!;

        public ICollection<BoardTask> AssignedTasks { get; set; } = [];
    }
}
