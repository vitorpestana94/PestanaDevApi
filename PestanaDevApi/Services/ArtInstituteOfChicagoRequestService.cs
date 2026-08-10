using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Services
{
    public class ArtInstituteOfChicagoRequestService: IArtInstituteOfChicagoRequestService
    {
        private readonly string _endPoint;
        private readonly IRequestService _http;
        private readonly IConfiguration _config;

        public ArtInstituteOfChicagoRequestService(IRequestService http, IConfiguration configuration)
        {
            _http = http;
            _config = configuration;

            if (string.IsNullOrEmpty(_config["artInstituteOfChicago.endpoint"]))
                throw new InvalidOperationException("Art Institute Of Chicago API endpoint not configured!!");

            _endPoint = _config["artInstituteOfChicago.endpoint"]!;
        }

        public async Task<ArtInstituteOfChicagoSearchResponseDto?> SearchArtWorks(string search)
        {
            try
            {
                return await _http.GetAsync<ArtInstituteOfChicagoSearchResponseDto>($"{_endPoint}/search", queryParams: new() { { "q", search }, { "fields", "id,title,image_id" } });
            }
            catch
            {
                return null;
            }
        }
    }
}
