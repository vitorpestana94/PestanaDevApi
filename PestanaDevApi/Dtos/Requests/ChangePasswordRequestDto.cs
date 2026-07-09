using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public required string NewPassword { get; set; }
        
        [Required]
        public required string CurrentPassword { get; set; }
    }
}
