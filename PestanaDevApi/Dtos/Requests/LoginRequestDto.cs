using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string DeviceId { get; set; } = string.Empty;
    }
}
