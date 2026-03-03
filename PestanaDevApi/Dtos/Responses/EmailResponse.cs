using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class EmailResponse : DefaultResponse
    {
        public EmailResponse() : base()
        {
        }

        public EmailResponse(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
