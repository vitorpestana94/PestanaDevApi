using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task<EmailResponseDto> SendContactEmail(ContactEmailRequestDto request);
        Task<SendConfirmationCodeEmailResponseDto> SendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request);
        Task<SendConfirmationCodeEmailResponseDto> ResendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request);
    }
}
