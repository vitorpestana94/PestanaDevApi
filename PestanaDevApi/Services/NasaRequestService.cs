using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Services
{
    public class NasaRequestService: INasaRequestService
    {
        private readonly string _endPoint;
        private readonly string _key;
        private readonly IConfiguration _config;
        private readonly IRequestService _http;

        public NasaRequestService(IConfiguration configuration, IRequestService http)
        {
            _config = configuration;
            _http = http;

            if (string.IsNullOrEmpty(_config["nasa.key"]))
                throw new InvalidOperationException("Nasa key not configured!");

            if (string.IsNullOrEmpty(_config["nasa.endpoint"]))
                throw new InvalidOperationException("Nasa end-poin not configured!");

            _endPoint = _config["nasa.endpoint"]!;
            _key = _config["nasa.key"]!;
        }

        public async Task<NasaResponse?> GetNasaPictureOfToday()
        {
            try
            {
                return await _http.GetAsync<NasaResponse>(_endPoint, queryParams: GetQueryParams());
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
                return await _http.GetAsync<NasaResponse>(_endPoint, queryParams: GetQueryParams(date));
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
                return await _http.GetAsync<IEnumerable<NasaResponse>>(_endPoint, queryParams: GetQueryParams(startDate, endDate));
            }
            catch
            {
                return null;
            }
        }

        #region Private Methods
        /// <summary>
        /// Creates the default query parameters required for every NASA API request.
        /// </summary>
        /// <returns>
        /// A dictionary containing the API authentication key.
        /// </returns>
        private Dictionary<string, string> GetQueryParams() => new() { { "api_key" , _key}};

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
        private Dictionary<string, string> GetQueryParams(string date)
        {
            Dictionary<string, string> queryParams = GetQueryParams();

            queryParams.Add("date", date);

            return queryParams;
        }

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
        private Dictionary<string, string> GetQueryParams(string startDate, string endDate)
        {
            Dictionary<string, string> queryParams = GetQueryParams();

            queryParams.Add("start_date", startDate);
            queryParams.Add("end_date", endDate);

            return queryParams;
        }
        #endregion
    }
}
