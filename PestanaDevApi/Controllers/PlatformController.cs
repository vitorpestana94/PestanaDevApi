using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("platform/auth")]
    [ApiController]
    [AllowAnonymous]

    public class PlatformController : Controller
    {
        private readonly IPlatformService _platformService;

        public PlatformController(IPlatformService platformService) 
        { 
            _platformService = platformService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginOrSignUpWithProvider([FromBody] LoginOrSignUpWithPlatformRequestDto request)
        {
            AuthResponseDto response = await _platformService.LoginOrSignUpWithProvider(request);

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response.ApiTokens);
        }
    }
}
