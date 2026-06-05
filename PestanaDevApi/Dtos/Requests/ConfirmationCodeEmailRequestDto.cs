using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Dtos.Requests
{
    public class ConfirmationCodeEmailRequestDto : EmailRequest
    {
        public required ConfirmationCodeEmailTypeEnum ConfirmationCodeEmailType {  get; set; } 
    }
}
