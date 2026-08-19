using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces
{
    public interface IMetropolitanMuseumCacheService
    {
        void DeleteMetropolitamMuseumCache();
        void SetMetropolitanMuseumCache(GetArtWorkResponseDto artWorkResponse, string search);
        GetArtWorkResponseDto? GetMetropolitanMuseumCache(string search);
    }
}
