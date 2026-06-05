using System.ComponentModel.DataAnnotations;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Dtos.Requests
{
    public class LoginOrSignUpWithPlatformRequestDto
    {
        [Required]
        public PlatformEnum Platform { get; set; }

        [Required]
        public string DeviceId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
