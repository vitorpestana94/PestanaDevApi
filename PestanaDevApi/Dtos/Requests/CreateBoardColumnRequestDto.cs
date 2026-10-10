using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class CreateBoardColumnRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
