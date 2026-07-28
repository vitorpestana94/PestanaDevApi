using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Controllers
{
    [Route("nasa")]
    [ApiController]

    public class NasaController: Controller
    {
        public NasaController() 
        { 
        } // Dps preciso pesquisar uma maneira de fazer cache. Ou seja, se a foto solicitada estiver em cache, retorna. se é q realmente é preciso um cache aqui. se for para fazer cache, q seja da resposta da nasa.

        [HttpGet]
        public async Task<IActionResult> GetNasaAstronomyPicture([FromQuery] GetNasaAstronomyPictureRequest request)
        {
            return Ok();
        }
    }
}
