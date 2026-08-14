using System.Net;
using PestanaDevApi.Constants.Messages;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Services
{
    public class MetropolitanMuseumIntegrationService: IMetropolitanMuseumIntegrationService
    {
        private readonly IMetropolitanMuseumRequestService _requestService;

        public MetropolitanMuseumIntegrationService(IMetropolitanMuseumRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<GetArtWorkResponseDto> GetArtWork(string? search)
        {
            if (string.IsNullOrEmpty(search))
                return new GetArtWorkResponseDto(ErrorMessages.SearchEmpty);

            SearchArtWorksIdsResponseDto? searchResponse = await _requestService.SearchArtWorksIds(search);

            if (searchResponse == null)
                return new GetArtWorkResponseDto(HttpStatusCode.InternalServerError, ErrorMessages.ErrorRequestingMetropolitanMuseum);

            List<int> objectIds = searchResponse.ObjectIds.Take(35).ToList();

            MetropolitanMuseumSearchResponseDto?[] artworks = await Task.WhenAll(objectIds.Select(id => _requestService.GetArtWork(id.ToString())));

            if (artworks == null || artworks.Length == 0)
                return new GetArtWorkResponseDto(HttpStatusCode.InternalServerError, ErrorMessages.ErrorRequestingMetropolitanMuseum);

            return new GetArtWorkResponseDto(artworks!);
        }
    }
}
