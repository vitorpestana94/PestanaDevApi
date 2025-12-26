using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
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

            return Ok(await _loginService.Login(request));
        }
    }
}
