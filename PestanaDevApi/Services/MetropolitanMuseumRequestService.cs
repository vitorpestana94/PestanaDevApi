using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;
using Consts = PestanaDevApi.Constants.MetropolitanMuseumConstants;

namespace PestanaDevApi.Services
{
    public class MetropolitanMuseumRequestService: IMetropolitanMuseumRequestService
    {
        private readonly string _endPoint;
        private readonly IRequestService _http;
        private readonly IConfiguration _config;

        public MetropolitanMuseumRequestService(IRequestService http, IConfiguration configuration)
        {
            _http = http;
            _config = configuration;

            if (string.IsNullOrEmpty(_config["metropolitanMuseum.endpoint"]))
                throw new InvalidOperationException("Metropolitan Museum API endpoint not configured!!");

            _endPoint = _config["metropolitanMuseum.endpoint"]!;
        }

        public async Task<SearchArtWorksIdsResponseDto?> SearchArtWorksIds(string search)
        {
            try
            {
                return await _http.RequestAsync<SearchArtWorksIdsResponseDto>($"{_endPoint}{Consts.SearchPath}", queryParams: new() { { "q", search } });
            }
            catch
            {
                return null;
            }
        }

        public async Task<MetropolitanMuseumSearchResponseDto?> GetArtWork(string artWorkId)
        {
            try
            {
                return await _http.RequestAsync<MetropolitanMuseumSearchResponseDto>($"{_endPoint}{Consts.ObjectsPath}{artWorkId}");
            }
            catch
            {
                return null;
            }
        }
    }
}
