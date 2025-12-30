using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("login")]
    [ApiController]
    [AllowAnonymous]

    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            return HandleLoginResponse(await _loginService.Login(request));
        }


        [HttpPost("platform")]
        public async Task<IActionResult> LoginWithPlatform([FromBody] LoginWithPlatformRequestDto request)
        {
            return HandleLoginResponse(await _loginService.LoginWithProvider(request));
        }

        #region Private Methods
        private IActionResult HandleLoginResponse(LoginResponseDto response)
        {
            if (!response.IsSuccess)
                return Unauthorized(response.ErrorMessage);

            return Ok(response.ApiTokens);
        }
        #endregion
    }
}
