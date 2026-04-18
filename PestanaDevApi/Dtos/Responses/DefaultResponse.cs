using System.Net;
using PestanaDevApi.Utils;
using ResponseMessage = PestanaDevApi.Constants.SuccessMessages;
using System.Text.Json.Serialization;

namespace PestanaDevApi.Dtos.Responses
{
    public class DefaultResponse
    {
        public string Message { get; set; } = string.Empty;

        public HttpStatusCode StatusCode { get; set; }
        
        [JsonIgnore]
        public bool IsSuccess { get; set; }

        public DefaultResponse() 
        {
            IsSuccess  = true;
            StatusCode = HttpStatusCode.OK;
            Message = ResponseMessage.DefaultSuccessMessage;
        }

        public DefaultResponse(string message)
        {
            IsSuccess = true;
            StatusCode = HttpStatusCode.OK;
            Message = message;
        }

        public DefaultResponse(HttpStatusCode statusCode)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Message = GetHttpMessage.Get(statusCode);
        }

        public DefaultResponse(HttpStatusCode statusCode, string errorMessage)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Message = errorMessage;
        }
    }
}
