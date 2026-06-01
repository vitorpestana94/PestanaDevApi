using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("auth")]
    [ApiController]
    [AllowAnonymous]

    public class AuthController: Controller
    {
        private readonly ILoginService _loginService;
        private readonly ISignUpService _signUpService;
        private readonly IPlatformService _platformService;
        private readonly IForgotPasswordService _forgotPasswordService;

        public AuthController(ILoginService loginService, ISignUpService signUpService, 
            IPlatformService platformService, IForgotPasswordService forgotPasswordService)
        {
            _loginService = loginService;
            _signUpService = signUpService;
            _platformService = platformService;
            _forgotPasswordService = forgotPasswordService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            AuthResponseDto response = await _loginService.Login(request);

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response.ApiTokens);
        }

        [HttpPost("oauth")]
        public async Task<IActionResult> LoginOrSignUpWithProvider([FromBody] LoginOrSignUpWithPlatformRequestDto request)
        {
            AuthResponseDto response = await _platformService.LoginOrSignUpWithProvider(request);

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response.ApiTokens);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto request)
        {
            SignUpResponseDto response = await _signUpService.SignUp(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response.ApiTokens);
        }

        [HttpGet("isEmailRegistered/{email}")]
        public async Task<IActionResult> IsEmailRegistered([FromRoute] string email)
        {
            IsEmailAlreadyRegisteredResponseDto response = await _signUpService.IsEmailAlreadyRegistered(email);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response.IsRegistered);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
        {
            ForgotPasswordResponseDto response = await _forgotPasswordService.ForgotPassword(requestDto);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
