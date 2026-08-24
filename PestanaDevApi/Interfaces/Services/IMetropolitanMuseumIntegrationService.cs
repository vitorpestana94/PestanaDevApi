using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IMetropolitanMuseumIntegrationService
    {
        /// <summary>
        /// Retrieves artworks matching the requested search term from the cache,
        /// or requests them from the Metropolitan Museum API if no cached data is available.
        /// Uses a semaphore to limit concurrent requests to the Metropolitan Museum API.
        /// The maxium requests per second defined bu Metropolitan Museum it's 80.
        /// </summary>
        /// <param name="search">
        /// Search term used to find artworks.
        /// </param>
        /// <returns>
        /// A response containing the artworks matching the search term,
        /// either from the cache or from the Metropolitan Museum API.
        /// </returns>
        Task<GetArtWorkResponseDto> GetArtWork(string search);

        /// <summary>
        /// Refreshes the Metropolitan Museum cache by clearing the existing cached data
        /// and retrieving the default set of artworks to repopulate the cache.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous cache refresh operation.
        /// </returns>
        Task RefreshMetrpolitanMuseumCache();
    }
}
