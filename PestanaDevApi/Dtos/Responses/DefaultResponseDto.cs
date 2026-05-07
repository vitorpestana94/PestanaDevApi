using System.Net;
using PestanaDevApi.Utils;
using ResponseMessage = PestanaDevApi.Constants.Messages.SuccessMessages;
using System.Text.Json.Serialization;

namespace PestanaDevApi.Dtos.Responses
{
    public class DefaultResponseDto
    {
        public string Message { get; set; } = string.Empty;

        public HttpStatusCode StatusCode { get; set; }
        
        [JsonIgnore]
        public bool IsSuccess { get; set; }

        public DefaultResponseDto() 
        {
            IsSuccess  = true;
            StatusCode = HttpStatusCode.OK;
            Message = ResponseMessage.DefaultSuccessMessage;
        }

        public DefaultResponseDto(string message)
        {
            IsSuccess = true;
            StatusCode = HttpStatusCode.OK;
            Message = message;
        }

        public DefaultResponseDto(HttpStatusCode statusCode)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Message = GetHttpMessage.Get(statusCode);
        }

        public DefaultResponseDto(HttpStatusCode statusCode, string errorMessage)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Message = errorMessage;
        }
    }
}
