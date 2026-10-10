using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class CreateBoardRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public CreateColumnNames ColumnsNames { get; set; } = new();

        [Required]
        public CreateBoardTask BoardTask { get; set; } = new();
    }

    public class CreateColumnNames
    {
        [Required]
        public string ColumnNameOne { get; set; } = string.Empty;
        
        [Required]
        public string ColumnNameTwo { get; set; } = string.Empty;

        [Required] 
        public string ColumnNameThree { get; set; } = string.Empty;
        
        [Required] 
        public string ColumnNameFour { get; set; } = string.Empty;
        
        [Required] 
        public string ColumnNameFive { get; set; } = string.Empty;
    }

    public class CreateBoardTask
    {
        [Required] 
        public string TaskName { get; set; } = string.Empty;
        
        [Required] 
        public string TaskType { get; set; } = string.Empty;

        [Required]
        public string TaskDescription { get; set; } = string.Empty;
    }
}
