namespace PestanaDevApi.Dtos.Requests
{
    public class ForgotPasswordRequestDto
    {
        public required string Email { get; set; } = string.Empty;
        public required string NewPassword { get; set; } = string.Empty;
    }
}
