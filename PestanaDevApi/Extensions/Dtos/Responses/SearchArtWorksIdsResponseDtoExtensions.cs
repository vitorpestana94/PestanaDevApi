using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Extensions.Dtos.Responses
{
    public static class SearchArtWorksIdsResponseDtoExtensions
    {
        public static bool TheresNoData(this SearchArtWorksIdsResponseDto? dto) => dto == null || dto.Total == 0 || dto.ObjectIds == null;
        public static List<int> GetIds(this SearchArtWorksIdsResponseDto dto, int idsAmount = 35) => dto.ObjectIds.Take(idsAmount).ToList();
    }
}
