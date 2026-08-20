using System.Net;
using PestanaDevApi.Constants.Messages;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Extensions.Dtos.Responses;
using Consts = PestanaDevApi.Constants.MetropolitanMuseumConstants;

namespace PestanaDevApi.Services
{
    public class MetropolitanMuseumIntegrationService: IMetropolitanMuseumIntegrationService
    {
        private readonly IMetropolitanMuseumRequestService _requestService;
        private readonly IMetropolitanMuseumCacheService _cacheService;
        private readonly ISemaphoreService _semaphoreService;

        public MetropolitanMuseumIntegrationService(IMetropolitanMuseumRequestService requestService, IMetropolitanMuseumCacheService cacheService, ISemaphoreService semaphoreService)
        {
            _requestService = requestService;
            _cacheService = cacheService;
            _semaphoreService = semaphoreService;
        }

        public async Task<GetArtWorkResponseDto> GetArtWork(string search)
        {
            return await _semaphoreService.Work(() => GetArts(search));
        }

        public async Task RefreshMetrpolitanMuseumCache()
        {
            _cacheService.DeleteMetropolitamMuseumCache();

            await GetArtWork(Consts.DefaultSearchTerm);
        }

        #region Private Methods
        /// <summary>
        /// Retrieves artworks matching the requested search term from the cache,
        /// or requests them from the Metropolitan Museum API if no cached data is available.
        /// </summary>
        /// <param name="search">
        /// Search term used to find artworks.
        /// </param>
        /// <returns>
        /// A response containing the artworks matching the search term.
        /// </returns>
        private async Task<GetArtWorkResponseDto> GetArts(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new GetArtWorkResponseDto(ErrorMessages.SearchEmpty);

            search = search.Trim();

            return _cacheService.GetMetropolitanMuseumCache(search) ?? await RequestMetropolitanMuseum(search);
        }

        /// <summary>
        /// Requests artworks matching the specified search term from the Metropolitan Museum API,
        /// retrieves their details in parallel, and caches the resulting response.
        /// </summary>
        /// <param name="search">
        /// Search term used to find artworks.
        /// </param>
        /// <returns>
        /// A response containing the artworks matching the search term,
        /// or a not found response if no artworks are available.
        /// </returns>
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

        /// <summary>
        /// Retrieves artwork details for multiple artwork IDs in parallel
        /// from the Metropolitan Museum API.
        /// </summary>
        /// <param name="artWorksId">
        /// List of artwork IDs to retrieve.
        /// </param>
        /// <returns>
        /// An array containing the artwork details for the requested IDs.
        /// </returns>
        private async Task<MetropolitanMuseumSearchResponseDto?[]> RequestArtWorkDataInParallel(List<int> artWorksId) => 
            await Task.WhenAll(artWorksId.Select(id => _requestService.GetArtWork(id.ToString())));
        #endregion
    }
}
