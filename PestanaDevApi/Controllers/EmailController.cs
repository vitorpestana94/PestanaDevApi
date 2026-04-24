using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services.Email;

namespace PestanaDevApi.Controllers
{
    [Route("email")] 
    [ApiController]

    public class EmailController: Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("contact")]
        [AllowAnonymous]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactEmailRequestDto requestDto)
        {
            EmailResponse response = await _emailService.SendContactEmail(requestDto);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> SendConfirmationCodeEmail([FromBody] ConfirmationCodeEmailRequestDto requestDto)
        {
            SendConfirmationCodeEmailResponseDto response = await _emailService.SendConfirmationCodeEmail(requestDto);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("confirmation/resend")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationCodeEmail([FromBody] ConfirmationCodeEmailRequestDto requestDto)
        {
            SendConfirmationCodeEmailResponseDto response = await _emailService.ResendConfirmationCodeEmail(requestDto);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
