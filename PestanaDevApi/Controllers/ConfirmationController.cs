using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("confirmation")]
    [ApiController]

    public class ConfirmationController : Controller
    {
        private readonly IConfirmationCodeService _service;

        public ConfirmationController(IConfirmationCodeService service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CheckConfirmationCode([FromBody] CheckConfirmationCodeRequest request)
        {
            CheckConfirmationCodeResponse response = await _service.IsConfirmationCodeValid(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
