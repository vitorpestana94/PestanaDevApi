using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class CaptchaRequestDto
    {
        [Required]
        public string CaptchaToken { get; set; } = string.Empty;
    }
}
