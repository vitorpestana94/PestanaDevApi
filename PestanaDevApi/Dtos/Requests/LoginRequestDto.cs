namespace PestanaDevApi.Dtos.Requests
{
    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string DeviceId { get; set; }

    }
}
