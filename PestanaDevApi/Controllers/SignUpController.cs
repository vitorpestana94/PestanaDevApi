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
            SignUpResponseDto respoonse = await _signUpService.SignUp(request);

            if (!respoonse.IsSuccess)
                return BadRequest(respoonse.ErrorMessage);

            return Ok(respoonse.ApiTokens);
        }
    }
}
