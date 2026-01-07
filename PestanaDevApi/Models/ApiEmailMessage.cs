using System.Net.Mail;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Constants;

namespace PestanaDevApi.Models
{
    public class ApiEmailMessage : MailMessage
    {
        public ApiEmailMessage() 
        { 
        }

        public ApiEmailMessage(string emailAddress, string body, string subject)
        {
            From = new MailAddress(emailAddress);
            To.Add(emailAddress);
            Subject = subject;
            Body = body;
            IsBodyHtml = true;
        }
    }
}
