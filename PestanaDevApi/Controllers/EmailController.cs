using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services.Email;

namespace PestanaDevApi.Controllers
{
    [Route("email")]
    [ApiController]
    [AllowAnonymous]

    public class EmailController: Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("contact")]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactEmailRequestDto requestDto)
        {
            EmailResponse response = await _emailService.SendContactEmail(requestDto);

            if (!response.IsSuccess)
                return BadRequest(response.ErrorMessage);

            return Ok(response);
        }
    }
}
