using System.Net.Mail;
using PestanaDevApi.Constants.Email;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Models
{
    public class ApiEmailMessage : MailMessage
    {
        public ApiEmailMessage() 
        { 
        }

        public ApiEmailMessage(string fromEmaillAddress, string body)
        {
            From = new MailAddress(fromEmaillAddress);
            To.Add(fromEmaillAddress);
            Subject = EmailConstants.ContactEmailSubject;
            Body = body;
            IsBodyHtml = true;
        }

        public ApiEmailMessage(ContactEmailRequestDto request, string fromEmaillAddress, string body)
        {
            From = new MailAddress(fromEmaillAddress);
            To.Add(request.ClientEmail);
            Subject = EmailConstants.ContactEmailSubject;
            Body = body;
            IsBodyHtml = true;
        }
    }
}
