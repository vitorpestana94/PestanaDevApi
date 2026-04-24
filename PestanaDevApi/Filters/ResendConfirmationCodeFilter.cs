using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PestanaDevApi.Constants;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Filters
{
    public class ResendConfirmationCodeFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            ClaimsPrincipal user = context.HttpContext.User;

            string purpose = user.FindFirst(ClaimsConstants.Purpose)?.Value ?? "";
            string issuedAt = user.FindFirst(ClaimsConstants.IssuedAt)?.Value ?? "";

            if (purpose != ClaimsConstants.EmailConfirmation || ApiLib.IsInvalidSecondsGap(issuedAt))
            {
                context.Result = new UnauthorizedObjectResult(ErrorMessages.Unauthorized);

                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
