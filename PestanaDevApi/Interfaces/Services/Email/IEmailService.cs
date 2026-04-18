using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task<EmailResponse> SendContactEmail(ContactEmailRequestDto request);
        Task<EmailResponse> SendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request);
    }
}
