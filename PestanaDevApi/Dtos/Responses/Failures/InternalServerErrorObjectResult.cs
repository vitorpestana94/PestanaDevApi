using Microsoft.AspNetCore.Mvc;

namespace PestanaDevApi.Dtos.Responses.Failures
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object? value): base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}