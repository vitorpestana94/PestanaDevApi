using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Extensions.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("art-institute-of-chicago")]
    [ApiController]
    [AllowAnonymous]

    public class ArtInstituteOfChicagoController : Controller
    {
        private readonly IArtInstituteOfChicagoIntegrationService _service;

        public ArtInstituteOfChicagoController(IArtInstituteOfChicagoIntegrationService service)
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
