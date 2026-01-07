using System.Net;
using System.Net.Mail;

namespace PestanaDevApi.Models
{
    public class ApiSmtpClient : SmtpClient
    {
        public ApiSmtpClient() 
        { 
        }

        public ApiSmtpClient(string smtp, string emailAddress, string appPassword) : base(smtp, 587)
        {
           Credentials = new NetworkCredential(emailAddress, appPassword);
           EnableSsl = true;
           DeliveryMethod = SmtpDeliveryMethod.Network;
        }
    }
}
