using System.Net;
using PestanaDevApi.Constants;

namespace PestanaDevApi.Dtos.Responses
{
    public class DefaultResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }

        public DefaultResponse() 
        {
            IsSuccess  = true;
            StatusCode = HttpStatusCode.OK;
        }

        public DefaultResponse(HttpStatusCode statusCode)
        {
            IsSuccess = false;
            ErrorMessage = GetErrorMessage(statusCode);
            StatusCode = statusCode;
        }

        public DefaultResponse(HttpStatusCode statusCode, string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        private static string GetErrorMessage(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => ErrorMessages.BadRequest,
                HttpStatusCode.Unauthorized => ErrorMessages.Unauthorized,
                _ => ErrorMessages.DefaultMessage
            };
        }
    }
}
