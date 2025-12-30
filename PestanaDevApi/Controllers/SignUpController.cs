using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Services;

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
            ApiToken? apiToken = await _signUpService.SignUp(request);

            if (apiToken == null)
                return BadRequest("Email format is invalid.");

            return Ok(apiToken);
        }
    }
}
