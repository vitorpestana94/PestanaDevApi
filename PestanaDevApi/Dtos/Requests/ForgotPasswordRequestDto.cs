using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        public required string Email { get; set; } = string.Empty;

        [Required]
        public required string NewPassword { get; set; } = string.Empty;
    }
}
