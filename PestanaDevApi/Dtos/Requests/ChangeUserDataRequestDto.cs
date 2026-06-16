namespace PestanaDevApi.Dtos.Requests
{
    public class ChangeUserDataRequestDto: CaptchaRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
