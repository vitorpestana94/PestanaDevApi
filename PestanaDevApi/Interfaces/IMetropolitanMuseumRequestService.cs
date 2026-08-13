using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces
{
    public interface IMetropolitanMuseumRequestService
    {
        Task<MetropolitanMuseumSearchResponseDto?> GetArtWork(string artWorkId);
        Task<SearchArtWorksIdsResponseDto?> SearchArtWorksIds(string search);
    }
}
