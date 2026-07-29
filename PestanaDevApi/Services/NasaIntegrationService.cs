using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Extensions.Dtos.Requests;
using PestanaDevApi.Constants.Messages;
using System.Net;

namespace PestanaDevApi.Services
{
    public class NasaIntegrationService: INasaIntegrationService
    {
        private readonly INasaRequestService _requestService;

        public NasaIntegrationService(INasaRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<NasaAstronomyPictureOfDayResponseDto> GetNasaAstronomyPictureOfDay(GetNasaAstronomyPictureOfDayRequest request)
        {
            if (request.IsSpecificDatePicture())
                return await GetTheAstronomyPictureOfTheDay(request);

            return await GetTheAstronomyPictureOfToday();
        }

        public async Task<NasaAstronomyPictureOfPeriodResponseDto> GetNasaAstronomyPictureOfPeriod(GetNasaAstronomyPictureOfPeriodRequest request)
        {
            if (request.CheckStartAndEndDates())
                return new(ErrorMessages.InvalidStartOrEndDate);

            return new();
        }

        #region Private Methods
        private async Task<NasaAstronomyPictureOfDayResponseDto> GetTheAstronomyPictureOfToday()
        {
            NasaResponse? response = await _requestService.GetNasaPictureOfToday();

            return response == null ? new(HttpStatusCode.InternalServerError, ErrorMessages.NasaResponseWithErrors) : new(response);
        }

        private async Task<NasaAstronomyPictureOfDayResponseDto> GetTheAstronomyPictureOfTheDay(GetNasaAstronomyPictureOfDayRequest request)
        {
            if (request.CheckSpecificDate())
                return new(ErrorMessages.InvalidDate);

            return new();
        }
        #endregion
    }
}
