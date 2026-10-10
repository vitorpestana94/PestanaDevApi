using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Extensions;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Services
{
    public class NasaRequestService: INasaRequestService
    {
        private readonly string _endPoint;
        private readonly IConfiguration _config;
        private readonly IRequestService _http;

        public NasaRequestService(IConfiguration configuration, IRequestService http)
        {
            _config = configuration;
            _http = http;

            if (string.IsNullOrEmpty(_config["nasa.endpoint"]))
                throw new InvalidOperationException("Nasa end-point not configured!");

            _endPoint = _config["nasa.endpoint"]!;
        }

        public async Task<NasaResponse?> GetNasaPictureOfToday()
        {
            try
            {
                return await _http.RequestAsync<NasaResponse>(GetNasaEndPointWithDate());
            }
            catch
            {
                return null;
            }
        }

        public async Task<NasaResponse?> GetNasaPictureOfDay(string date)
        {
            try
            {
                return await _http.RequestAsync<NasaResponse>(GetNasaEndPointWithDate(date));
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<NasaResponse>?> GetNasaPicturesOfPeriod(string startDate, string endDate)
        {
            try
            {
                return await _http.RequestAsync<IEnumerable<NasaResponse>>(_endPoint, queryParams: GetQueryParams(startDate, endDate));
            }
            catch
            {
                return null;
            }
        }

        #region Private Methods
        /// <summary>
        /// Creates the query parameters required to retrieve the Astronomy Picture of the Day
        /// for a specific date.
        /// </summary>
        /// <param name="date">
        /// The requested date in the format expected by the NASA API (yyyy-MM-dd).
        /// </param>
        /// <returns>
        /// A dictionary containing the API key and the requested date.
        /// </returns>
        private string GetNasaEndPointWithDate() => $"{_endPoint}/{DateTime.Today.GetNasaDateFormat()}";

        private string GetNasaEndPointWithDate(string date) => $"{_endPoint}/{date}";

        /// <summary>
        /// Creates the query parameters required to retrieve Astronomy Pictures of the Day
        /// within a specified date range.
        /// </summary>
        /// <param name="startDate">
        /// The start date of the desired period in the format expected by the NASA API (yyyy-MM-dd).
        /// </param>
        /// <param name="endDate">
        /// The end date of the desired period in the format expected by the NASA API (yyyy-MM-dd).
        /// </param>
        /// <returns>
        /// A dictionary containing the API key, start date, and end date.
        /// </returns>
        private static Dictionary<string, string> GetQueryParams(string startDate, string endDate)
        {
            Dictionary<string, string> queryParams = new()
            {
                { "date_from", DateLib.GetNasaDate(startDate) },
                { "date_to", DateLib.GetNasaDate(endDate) }
            };

            return queryParams;
        }
        #endregion
    }
}
