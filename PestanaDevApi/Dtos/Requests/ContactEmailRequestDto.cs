using System.ComponentModel.DataAnnotations;

namespace PestanaDevApi.Dtos.Requests
{
    public class ContactEmailRequestDto : EmailRequest
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        public string ClientMessage { get; set; } = string.Empty;
    }
}
