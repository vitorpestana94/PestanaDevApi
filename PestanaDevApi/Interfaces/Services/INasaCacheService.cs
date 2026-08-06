using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface INasaCacheService
    {
        /// <summary>
        /// Retrieves the cached Astronomy Picture of the Day response.
        /// </summary>
        /// <returns>
        /// The cached Astronomy Picture of the Day response if available;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        NasaAstronomyPictureOfDayResponseDto? GetNasaAstronomyPictureOfTodayCache();

        /// <summary>
        /// Stores the Astronomy Picture of the Day response in the cache.
        /// </summary>
        /// <param name="nasaResponse">
        /// The response to be cached.
        /// </param>
        void SetNasaAstronomyPictureOfTodayCache(NasaAstronomyPictureOfDayResponseDto nasaResponse);

        /// <summary>
        /// Retrieves the cached Astronomy Pictures response for a period.
        /// </summary>
        /// <returns>
        /// The cached Astronomy Pictures response for a period if available;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        NasaAstronomyPicturesOfPeriodResponseDto? GetNasaAstronomyPictureOfThePeriodCache();

        /// <summary>
        /// Stores the Astronomy Pictures response for a period in the cache.
        /// </summary>
        /// <param name="nasaResponse">
        /// The response to be cached.
        /// </param>
        void SetNasaAstronomyPictureOfThePeriodCache(NasaAstronomyPicturesOfPeriodResponseDto nasaResponse);

        void DeleteNasaCache();
    }
}
