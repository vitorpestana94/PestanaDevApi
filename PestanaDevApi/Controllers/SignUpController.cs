using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("sign-up")]
    [ApiController]
    [AllowAnonymous]

    public class SignUpController : Controller
    {
        private readonly ISignUpService _signUpService;

        public SignUpController(ISignUpService signUpService)
        {
            _signUpService = signUpService;
        }

        [HttpPost]
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
    }
}
