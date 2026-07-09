using Microsoft.AspNetCore.Mvc;

namespace PestanaDevApi.Dtos.Responses.Failures
{
    public class ForbiddenObjectResult: ObjectResult
    {
        public ForbiddenObjectResult(object? value): base(value)
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
