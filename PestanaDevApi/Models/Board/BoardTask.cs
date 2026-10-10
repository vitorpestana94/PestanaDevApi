namespace PestanaDevApi.Models.Board
{
    public class BoardTask
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid BoardId { get; set; } = Guid.Empty;

        public Guid ColumnId { get; set; } = Guid.Empty;

        public Guid TaskOwnerId { get; set; } = Guid.Empty;

        public string? TaskType { get; set; }

        public string TaskName { get; set; } = null!;

        public string? TaskDescription { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public BoardColumn Column { get; set; } = null!;

        public BoardUser? TaskOwner { get; set; }
    }
}
