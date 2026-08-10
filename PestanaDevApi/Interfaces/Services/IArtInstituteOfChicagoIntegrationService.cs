using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IArtInstituteOfChicagoIntegrationService
    {
        Task<GetArtWorkResponseDto> GetArtWork(string? search);
    }
}
