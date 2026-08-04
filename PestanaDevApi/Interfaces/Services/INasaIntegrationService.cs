using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface INasaIntegrationService
    {
        /// <summary>
        /// Retrieves NASA's Astronomy Picture of the Day (APOD).
        /// If a specific date is provided, returns the picture for that date;
        /// otherwise, returns today's picture. Nasa's API doc can be found here: https://api.nasa.gov/
        /// </summary>
        /// <param name="request">
        /// Request containing the optional date for the desired picture.
        /// </param>
        /// <returns>
        /// A response containing the Astronomy Picture of the Day or an error response.
        /// </returns>
        Task<NasaAstronomyPictureOfDayResponseDto> GetNasaAstronomyPictureOfDay(GetNasaAstronomyPictureOfDayRequest request);

        /// <summary>
        /// Retrieves NASA's Astronomy Pictures of the Day (APOD) for a specified date range.
        /// Nasa's API doc can be found here: https://api.nasa.gov/
        /// </summary>
        /// <param name="request">
        /// Request containing the start and end dates of the desired period.
        /// </param>
        /// <returns>
        /// A response containing the collection of Astronomy Pictures of the Day
        /// for the specified period or an error response.
        /// </returns>
        Task<NasaAstronomyPicturesOfPeriodResponseDto> GetNasaAstronomyPictureOfPeriod(GetNasaAstronomyPicturesOfPeriodRequest request);

        Task RefreshNasaCache();
    }
}
