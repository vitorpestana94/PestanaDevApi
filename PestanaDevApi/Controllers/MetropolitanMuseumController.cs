using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Extensions.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("metropolitan-museum")]
    [ApiController]
    [AllowAnonymous]

    public class MetropolitanMuseumController : Controller
    {
        private readonly IMetropolitanMuseumIntegrationService _service;

        public MetropolitanMuseumController(IMetropolitanMuseumIntegrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtWork([FromQuery] GetArtWorkRequest request)
        {
            GetArtWorkResponseDto response = await _service.GetArtWork(request.Search);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response.ArtData);
        }
    }
}
