using System.Net;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Extensions.Dtos.Responses
{
    public static class DefaultResponseExtensions
    {
        public static IActionResult HandleFailure(this DefaultResponseDto dto)
        {
            return dto.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundObjectResult(dto),
                HttpStatusCode.InternalServerError => new ObjectResult(dto) { StatusCode = 500 },
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(dto),
                _ => new BadRequestObjectResult(dto)
            };
        }
    }
}
