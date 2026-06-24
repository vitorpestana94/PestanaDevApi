using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public required string DeviceId { get; set; }
    }
}
