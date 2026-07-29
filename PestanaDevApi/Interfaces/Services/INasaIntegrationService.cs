using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface INasaIntegrationService
    {
        Task<NasaAstronomyPictureOfDayResponseDto> GetNasaAstronomyPictureOfDay(GetNasaAstronomyPictureOfDayRequest request);
    }
}
