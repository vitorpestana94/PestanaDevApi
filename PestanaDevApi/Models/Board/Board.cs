namespace PestanaDevApi.Models.Board
{
    public class Board
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid OwnerId { get; set; } = Guid.Empty;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User Owner { get; set; } = null!;

        public ICollection<BoardUser> Users { get; set; } = [];

        public ICollection<BoardColumn> Columns { get; set; } = [];
    }
}
