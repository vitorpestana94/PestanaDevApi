using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces
{
    public interface IMetropolitanMuseumRequestService
    {
        /// <summary>
        /// Retrieves detailed information about an artwork from the Metropolitan Museum API.
        /// </summary>
        /// <param name="artWorkId">
        /// ID of the artwork to retrieve.
        /// </param>
        /// <returns>
        /// A response containing the artwork details,
        /// or null if the request fails.
        /// </returns>
        Task<MetropolitanMuseumSearchResponseDto?> GetArtWork(string artWorkId);

        /// <summary>
        /// Searches the Metropolitan Museum API for artworks matching the specified search term.
        /// </summary>
        /// <param name="search">
        /// Search term used to find artworks.
        /// </param>
        /// <returns>
        /// A response containing the IDs of artworks matching the search term,
        /// or null if the request fails.
        /// </returns>
        Task<SearchArtWorksIdsResponseDto?> SearchArtWorksIds(string search);
    }
}
