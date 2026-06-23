using System.Net;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Dtos.Responses.Failures;

namespace PestanaDevApi.Extensions.Dtos.Responses
{
    public static class DefaultResponseExtensions
    {
        public static IActionResult HandleFailure(this DefaultResponseDto dto)
        {
            return dto.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundObjectResult(dto),
                HttpStatusCode.InternalServerError => new InternalServerErrorObjectResult(dto),
                HttpStatusCode.Forbidden => new ForbiddenObjectResult(dto),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(dto),
                _ => new BadRequestObjectResult(dto)
            };
        }
    }
}
