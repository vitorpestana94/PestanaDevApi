namespace PestanaDevApi.Dtos.Requests
{
    public class ChangePasswordRequestDto
    {
        public required string NewPassword { get; set; }
        public required string CurrentPassword { get; set; }

    }
}
