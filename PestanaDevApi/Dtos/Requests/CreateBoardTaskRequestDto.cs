using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class CreateBoardTaskRequestDto
    {
        [Required]
        public Guid ColumnId { get; set; }

        /// <summary>
        /// Optional. When not sent, the user creating the task becomes its owner.
        /// </summary>
        public Guid? TaskOwnerId { get; set; }

        [Required]
        [MaxLength(150)]
        public string TaskName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? TaskType { get; set; }

        public string? TaskDescription { get; set; }

        public DateTime? Deadline { get; set; }
    }
}
