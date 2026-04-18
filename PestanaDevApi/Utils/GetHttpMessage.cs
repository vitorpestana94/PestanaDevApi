using System.Net;
using ErrorMessage = PestanaDevApi.Constants.ErrorMessages;

namespace PestanaDevApi.Utils
{
    public class GetHttpMessage
    {
        /// <summary>
        /// Returns the error message by the thrown error and it status code.
        /// <param name="statusCode">The thrown error status code</param>
        /// <returns>
        /// Error message
        /// </returns>
        /// </summary>
        public static string Get(int statusCode)
        {
            return statusCode switch
            {
                400 => ErrorMessage.BadRequest,
                401 => ErrorMessage.Unauthorized,
                404 => ErrorMessage.NotFound,
                415 => ErrorMessage.UnsupportedMediaType,
                500 => ErrorMessage.InternalServerError,
                _ => ErrorMessage.DefaultMessage
            };
        }

        /// <summary>
        /// Returns the error message by the thrown error and it status code.
        /// <param name="statusCode">The thrown error status code</param>
        /// <returns>
        /// Error message
        /// </returns>
        /// </summary>
        public static string Get(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => ErrorMessage.BadRequest,
                HttpStatusCode.Unauthorized => ErrorMessage.Unauthorized,
                HttpStatusCode.NotFound => ErrorMessage.NotFound,
                HttpStatusCode.UnsupportedMediaType => ErrorMessage.UnsupportedMediaType,
                HttpStatusCode.InternalServerError => ErrorMessage.InternalServerError,
                _ => ErrorMessage.DefaultMessage
            };
        }
    }
}
