using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface INasaIntegrationService
    {
        NasaAstronomyPictureOfTheDayResponseDto GetNasaAstronomyPicture(GetNasaAstronomyPictureRequest request);
    }
}
