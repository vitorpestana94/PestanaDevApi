namespace PestanaDevApi.Dtos.Requests
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public required string Token { get; set; }
        public required string DeviceId { get; set; }
    }
}
