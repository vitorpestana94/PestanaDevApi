using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface INasaRequestService
    {
        Task<NasaResponse?> GetNasaPictureOfToday();
    }
}
