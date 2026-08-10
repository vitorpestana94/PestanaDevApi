using System.Net;
using PestanaDevApi.Constants.Messages;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Services
{
    public class ArtInstituteOfChicagoIntegrationService: IArtInstituteOfChicagoIntegrationService
    {
        private readonly IArtInstituteOfChicagoRequestService _requestService;

        public ArtInstituteOfChicagoIntegrationService(IArtInstituteOfChicagoRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<GetArtWorkResponseDto> GetArtWork(string? search)
        {
            if (string.IsNullOrEmpty(search))
                return new GetArtWorkResponseDto(ErrorMessages.SearchEmpty);

            ArtInstituteOfChicagoSearchResponseDto? response = await _requestService.SearchArtWorks(search);

            if (response == null)
                return new GetArtWorkResponseDto(HttpStatusCode.InternalServerError, ErrorMessages.ErrorRequestingArtInstituteOfChicagoApi);

            return new GetArtWorkResponseDto(response);
        }
    }
}
