using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces.Services.Email
{
    public interface IEmailTemplateService
    {
        Task<string> GetEmailTemplate(ContactEmailRequestDto request, bool isContactConfirmation = false);
        Task<string> GetEmailTemplate(ConfirmationCodeEmailRequestDto request, IEnumerable<int> confirmationCodes);
    }
}
