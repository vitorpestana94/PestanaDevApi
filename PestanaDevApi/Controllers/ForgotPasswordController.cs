using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("forgot-password")]
    [ApiController]
    [AllowAnonymous]

    public class ForgotPasswordController : Controller
    {
        IForgotPasswordService _service;

        public ForgotPasswordController(IForgotPasswordService service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
        {
            ForgotPasswordResponseDto response = await _service.ForgotPassword(requestDto);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
