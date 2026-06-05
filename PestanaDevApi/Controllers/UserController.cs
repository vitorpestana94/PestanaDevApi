using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("user")]
    [ApiController]
    [Authorize]

    public class UserController: Controller
    {
        private readonly IUserService _service;
        private Guid _userId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public UserController(IUserService service) 
        { 
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GetUserResponseDto response = await _service.GetUser(_userId);

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
