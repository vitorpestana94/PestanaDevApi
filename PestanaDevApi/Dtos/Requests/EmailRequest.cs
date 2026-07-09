using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class EmailRequest: CaptchaRequestDto
    {
        [Required]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        public string ClientLocale { get; set; } = string.Empty;
    }
}
