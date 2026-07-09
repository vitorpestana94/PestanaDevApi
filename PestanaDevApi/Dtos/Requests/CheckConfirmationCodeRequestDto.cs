using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class CheckConfirmationCodeRequestDto
    {
        [Required]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
