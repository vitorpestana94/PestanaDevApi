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
        } // Dps preciso pesquisar uma maneira de fazer cache. Ou seja, se a foto solicitada estiver em cache, retorna. se é q realmente é preciso um cache aqui. se for para fazer cache, q seja da resposta da nasa.

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
