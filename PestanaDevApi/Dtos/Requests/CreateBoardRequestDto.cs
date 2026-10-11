using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class CreateBoardRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public CreateColumnNames ColumnsNames { get; set; } = new();

        [Required]
        public CreateBoardTask BoardTask { get; set; } = new();
    }

    public class CreateColumnNames
    {
        [Required]
        [MaxLength(50)]
        public string ColumnNameOne { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string ColumnNameTwo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ColumnNameThree { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string ColumnNameFour { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string ColumnNameFive { get; set; } = string.Empty;
    }

    public class CreateBoardTask
    {
        [Required]
        [MaxLength(150)]
        public string TaskName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string TaskType { get; set; } = string.Empty;

        [Required]
        public string TaskDescription { get; set; } = string.Empty;
    }
}
