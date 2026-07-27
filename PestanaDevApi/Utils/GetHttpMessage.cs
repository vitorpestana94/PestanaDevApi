using System.Net;
using PestanaDevApi.Constants.Messages;

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
                400 => ErrorMessages.BadRequest,
                401 => ErrorMessages.Unauthorized,
                403 => ErrorMessages.Forbidden,
                404 => ErrorMessages.NotFound,
                415 => ErrorMessages.UnsupportedMediaType,
                500 => ErrorMessages.InternalServerError,
                _ => ErrorMessages.DefaultMessage
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
                HttpStatusCode.BadRequest => ErrorMessages.BadRequest,
                HttpStatusCode.Unauthorized => ErrorMessages.Unauthorized,
                HttpStatusCode.NotFound => ErrorMessages.NotFound,
                HttpStatusCode.Forbidden => ErrorMessages.Forbidden,
                HttpStatusCode.UnsupportedMediaType => ErrorMessages.UnsupportedMediaType,
                HttpStatusCode.InternalServerError => ErrorMessages.InternalServerError,
                _ => ErrorMessages.DefaultMessage
            };
        }
    }
}
