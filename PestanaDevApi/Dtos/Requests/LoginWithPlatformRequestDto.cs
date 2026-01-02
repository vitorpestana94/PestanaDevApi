using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Dtos.Requests
{
    public class LoginWithPlatformRequestDto
    {
        public required Platform Platform { get; set; }
        public required string DeviceId { get; set; }
        public required string Token { get; set; }
    }
}
