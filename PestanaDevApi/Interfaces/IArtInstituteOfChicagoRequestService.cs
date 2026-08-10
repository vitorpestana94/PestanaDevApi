using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces
{
    public interface IArtInstituteOfChicagoRequestService
    {
        Task<ArtInstituteOfChicagoSearchResponseDto?> SearchArtWorks(string search);
    }
}
