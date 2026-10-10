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

        #region Board
        [HttpPatch("{boardId}")]
        public async Task<IActionResult> UpdateBoard(Guid boardId, [FromBody] UpdateBoardRequestDto request)
        {
            UpdateBoardResponseDto response = await _service.UpdateBoard(UserId, boardId, request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpDelete("{boardId}")]
        public async Task<IActionResult> DeleteBoard(Guid boardId)
        {
            DeleteBoardResponseDto response = await _service.DeleteBoard(UserId, boardId);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }
        #endregion

        #region Tasks
        [HttpPatch("{boardId}/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid boardId, Guid taskId, [FromBody] UpdateBoardTaskRequestDto request)
        {
            UpdateBoardTaskResponseDto response = await _service.UpdateTask(UserId, boardId, taskId, request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpDelete("{boardId}/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid boardId, Guid taskId)
        {
            DeleteBoardTaskResponseDto response = await _service.DeleteTask(UserId, boardId, taskId);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }
        #endregion

        #region Columns
        [HttpPost("{boardId}/columns")]
        public async Task<IActionResult> CreateColumn(Guid boardId, [FromBody] CreateBoardColumnRequestDto request)
        {
            CreateBoardColumnResponseDto response = await _service.CreateColumn(UserId, boardId, request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpPatch("{boardId}/columns/{columnId}")]
        public async Task<IActionResult> UpdateColumn(Guid boardId, Guid columnId, [FromBody] UpdateBoardColumnRequestDto request)
        {
            UpdateBoardColumnResponseDto response = await _service.UpdateColumn(UserId, boardId, columnId, request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpDelete("{boardId}/columns/{columnId}")]
        public async Task<IActionResult> DeleteColumn(Guid boardId, Guid columnId)
        {
            DeleteBoardColumnResponseDto response = await _service.DeleteColumn(UserId, boardId, columnId);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }
        #endregion

        #region Members
        [HttpGet("{boardId}/members")]
        public async Task<IActionResult> GetBoardMembers(Guid boardId)
        {
            GetBoardMembersResponseDto response = await _service.GetBoardMembers(UserId, boardId);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpPost("{boardId}/members")]
        public async Task<IActionResult> AddBoardMember(Guid boardId, [FromBody] AddBoardMemberRequestDto request)
        {
            AddBoardMemberResponseDto response = await _service.AddBoardMember(UserId, boardId, request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }

        [HttpDelete("{boardId}/members/{userId}")]
        public async Task<IActionResult> RemoveBoardMember(Guid boardId, Guid userId)
        {
            RemoveBoardMemberResponseDto response = await _service.RemoveBoardMember(UserId, boardId, userId);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response);
        }
        #endregion
    }
}
