using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IMetropolitanMuseumIntegrationService
    {
        Task<GetArtWorkResponseDto> GetArtWork(string search);
    }
}
