using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Extensions;
using PestanaDevApi.Extensions.Dtos.Responses;

namespace PestanaDevApi.Controllers
{
    [Route("user")]
    [ApiController]
    [Authorize]

    public class UserController: Controller
    {
        private readonly IUserService _service;
        private Guid UserId => User.GetUserId();

        public UserController(IUserService service) 
        { 
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GetUserResponseDto response = await _service.GetUser(UserId);

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserData(ChangeUserDataRequestDto dto)
        {
            ChangeUserDataResponseDto response = await _service.ChangeUserData(dto, UserId);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            DeleteUserResponseDto response = await _service.DeleteUser(UserId);

            if (!response.IsSuccess)
               return response.HandleFailure();

            return Ok(response);
        }
    }
}
