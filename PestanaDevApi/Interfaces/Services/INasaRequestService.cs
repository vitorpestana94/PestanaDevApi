using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface INasaRequestService
    {
        /// <summary>
        /// Retrieves today's NASA Astronomy Picture of the Day (APOD).
        /// Nasa's API doc can be found here: https://api.nasa.gov/
        /// </summary>
        /// <returns>
        /// A <see cref="NasaResponse"/> containing today's picture if the request succeeds;
        /// otherwise, <c>null</c>.
        /// </returns>
        Task<NasaResponse?> GetNasaPictureOfToday();

        /// <summary>
        /// Retrieves the NASA Astronomy Picture of the Day (APOD) for a specific date.
        /// Nasa's API doc can be found here: https://api.nasa.gov/
        /// </summary>
        /// <param name="date">
        /// The date of the desired picture in the format expected by the NASA API (yyyy-MM-dd).
        /// </param>
        /// <returns>
        /// A <see cref="NasaResponse"/> containing the picture for the specified date if the request succeeds;
        /// otherwise, <c>null</c>.
        /// </returns>
        Task<NasaResponse?> GetNasaPictureOfDay(string date);

        /// <summary>
        /// Retrieves NASA Astronomy Pictures of the Day (APOD) for a specified date range.
        /// Nasa's API doc can be found here: https://api.nasa.gov/
        /// </summary>
        /// <param name="startDate">
        /// The start date of the desired period in the format expected by the NASA API (yyyy-MM-dd).
        /// </param>
        /// <param name="endDate">
        /// The end date of the desired period in the format expected by the NASA API (yyyy-MM-dd).
        /// </param>
        /// <returns>
        /// A collection of <see cref="NasaResponse"/> objects if the request succeeds;
        /// otherwise, <c>null</c>.
        /// </returns>
        Task<IEnumerable<NasaResponse>?> GetNasaPicturesOfPeriod(string startDate, string endDate);
    }
}
