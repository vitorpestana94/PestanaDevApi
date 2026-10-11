using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Extensions.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Controllers
{
    [Route("nasa")]
    [ApiController]
    [Authorize]

    public class NasaController: Controller
    {
        private readonly INasaIntegrationService  _service;

        public NasaController(INasaIntegrationService service) 
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetNasaAstronomyPictureOfDay([FromQuery] GetNasaAstronomyPictureOfDayRequest request)
        {
            NasaAstronomyPictureOfDayResponseDto response = await _service.GetNasaAstronomyPictureOfDay(request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response.NasaResponse);
        }

        [HttpGet("period")]
        public async Task<IActionResult> NasaAstronomyPicturesOfPeriod([FromQuery] GetNasaAstronomyPicturesOfPeriodRequest request)
        {
            NasaAstronomyPicturesOfPeriodResponseDto response = await _service.GetNasaAstronomyPictureOfPeriod(request);

            if (!response.IsSuccess)
                return response.HandleFailure();

            return Ok(response.NasaResponse);
        }
    }
}
