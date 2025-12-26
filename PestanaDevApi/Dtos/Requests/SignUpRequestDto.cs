namespace PestanaDevApi.Dtos.Requests
{
    public class SignUpRequestDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }

        public required string Password { get; set; }
        public required string DeviceId { get; set; }

        public string Picture { get; set; } = string.Empty;

    }
}
