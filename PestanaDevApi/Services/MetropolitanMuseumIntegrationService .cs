using System.Net;
using PestanaDevApi.Constants.Messages;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Extensions.Dtos.Responses;

namespace PestanaDevApi.Services
{
    public class MetropolitanMuseumIntegrationService: IMetropolitanMuseumIntegrationService
    {
        private readonly IMetropolitanMuseumRequestService _requestService;
        private readonly IMetropolitanMuseumCacheService _cacheService;

        public MetropolitanMuseumIntegrationService(IMetropolitanMuseumRequestService requestService, IMetropolitanMuseumCacheService cacheService)
        {
            _requestService = requestService;
            _cacheService = cacheService;
        }

        public async Task<GetArtWorkResponseDto> GetArtWork(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new GetArtWorkResponseDto(ErrorMessages.SearchEmpty);
            
            search = search.Trim();

            return _cacheService.GetMetropolitanMuseumCache(search) ?? await RequestMetropolitanMuseum(search);
        }

        #region Private Methods
        private async Task<GetArtWorkResponseDto> RequestMetropolitanMuseum(string search)
        {
            SearchArtWorksIdsResponseDto? searchResponse = await _requestService.SearchArtWorksIds(search);

            if (searchResponse.TheresNoData())
                return new GetArtWorkResponseDto(HttpStatusCode.NotFound, ErrorMessages.ArtWorksNotFound);

            MetropolitanMuseumSearchResponseDto?[] artworks = await RequestArtWorkDataInParallel(searchResponse!.GetIds());

            if (artworks == null || artworks.Length == 0)
                return new GetArtWorkResponseDto(HttpStatusCode.NotFound, ErrorMessages.ArtWorksNotFound);

            GetArtWorkResponseDto response = new(artworks!);

            _cacheService.SetMetropolitanMuseumCache(response, search);

            return response;
        }

        private async Task<MetropolitanMuseumSearchResponseDto?[]> RequestArtWorkDataInParallel(List<int> artWorksId ) => 
            await Task.WhenAll(artWorksId.Select(id => _requestService.GetArtWork(id.ToString())));
        #endregion
    }
}
