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
                return await _http.GetAsync<NasaResponse>(_endPoint, queryParams: GetNasaSecret());
            }
            catch
            {
                return null;
            }
        }

        #region Private Methods
        private Dictionary<string, string> GetNasaSecret() => new() { { "api_key" , _key}};
        #endregion
    }
}
