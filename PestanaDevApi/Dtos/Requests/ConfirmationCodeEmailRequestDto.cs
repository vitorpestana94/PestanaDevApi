using System.ComponentModel.DataAnnotations;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Dtos.Requests
{
    public class ConfirmationCodeEmailRequestDto : EmailRequest
    {
        [Required]
        public required ConfirmationCodeEmailTypeEnum ConfirmationCodeEmailType {  get; set; } 
    }
}
