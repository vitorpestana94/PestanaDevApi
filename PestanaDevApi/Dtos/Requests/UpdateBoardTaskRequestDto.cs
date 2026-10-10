namespace PestanaDevApi.Dtos.Requests
{
    public class UpdateBoardTaskRequestDto
    {
        public Guid? ColumnId { get; set; }
        public Guid? TaskOwnerId { get; set; }
        public string? TaskName { get; set; }
        public string? TaskType { get; set; }
        public string? TaskDescription { get; set; }
    }
}
