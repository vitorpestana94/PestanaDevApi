using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("board")]
    [ApiController]
    [Authorize]

    public class BoardController
    {
        private readonly IBoardService _service;

        public BoardController(IBoardService service)
        {
            _service = service;
        }


    }
}
