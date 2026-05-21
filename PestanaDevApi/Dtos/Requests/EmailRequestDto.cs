using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class EmailRequestDto
    {
        [Required]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        public string ClientLocale { get; set; } = string.Empty;
    }
}
