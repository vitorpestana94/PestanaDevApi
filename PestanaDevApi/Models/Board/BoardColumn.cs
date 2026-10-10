namespace PestanaDevApi.Models.Board
{
    public class BoardColumn
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid BoardId { get; set; } = Guid.Empty;

        public string Name { get; set; } = null!;

        public int Position { get; set; }

        public DateTime CreatedAt { get; set; }

        public Board Board { get; set; } = null!;

        public ICollection<BoardTask> Tasks { get; set; } = [];
    }
}
