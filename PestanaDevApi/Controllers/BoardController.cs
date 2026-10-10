using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Extensions.Dtos.Responses;
using PestanaDevApi.Extensions;

namespace PestanaDevApi.Controllers
{
    [Route("board")]
    [ApiController]
    [Authorize]

    public class BoardController: Controller
    {
        private readonly IBoardService _service;
        private Guid UserId => User.GetUserId();


        public BoardController(IBoardService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequestDto request)
        {
            CreateBoardResponseDto response = await _service.CreateBoard(UserId, request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBoardsIds()
        {
            GetBoardsResponseDto response = await _service.GetUserBoardsIds(UserId);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoardData(Guid id)
        {
            GetBoardDataResponseDto response = await _service.GetBoardData(id);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }
    }
}
