namespace PestanaDevApi.Dtos.Requests
{
    public class UpdateBoardTaskRequestDto
    {
        public Guid? ColumnId { get; set; }
        public Guid? TaskOwnerId { get; set; }
        public string? TaskName { get; set; }
        public string? TaskType { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// When true, removes the task deadline (a null Deadline only keeps the current one).
        /// </summary>
        public bool? ClearDeadline { get; set; }
    }
}
