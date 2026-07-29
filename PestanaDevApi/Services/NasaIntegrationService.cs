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

        public async Task<NasaAstronomyPicturesOfPeriodResponseDto> GetNasaAstronomyPictureOfPeriod(GetNasaAstronomyPicturesOfPeriodRequest request)
        {
            if (!request.AreBothDatesValids())
                return new(ErrorMessages.InvalidStartOrEndDate);

            IEnumerable<NasaResponse>? response = await _requestService.GetNasaPicturesOfPeriod(request.StartDate!, request.EndDate!);

            return response == null ? new(HttpStatusCode.InternalServerError, ErrorMessages.NasaResponseWithErrors) : new(response);
        }

        #region Private Methods
        /// <summary>
        /// Retrieves today's Astronomy Picture of the Day from the NASA API.
        /// </summary>
        /// <returns>
        /// A response containing today's Astronomy Picture of the Day
        /// or an error response.
        /// </returns>
        private async Task<NasaAstronomyPictureOfDayResponseDto> GetTheAstronomyPictureOfToday()
        {
            NasaResponse? response = await _requestService.GetNasaPictureOfToday();

            return response == null ? new(HttpStatusCode.InternalServerError, ErrorMessages.NasaResponseWithErrors) : new(response);
        }

        /// <summary>
        /// Retrieves the Astronomy Picture of the Day for a specific date.
        /// </summary>
        /// <param name="request">
        /// Request containing the desired date.
        /// </param>
        /// <returns>
        /// A response containing the Astronomy Picture of the Day for the specified date
        /// or an error response.
        /// </returns>
        private async Task<NasaAstronomyPictureOfDayResponseDto> GetTheAstronomyPictureOfTheDay(GetNasaAstronomyPictureOfDayRequest request)
        {
            if (request.IsDateNotValid())
                return new(ErrorMessages.InvalidDate);

            NasaResponse? response = await _requestService.GetNasaPictureOfDay(request.Date!);

            return response == null ? new(HttpStatusCode.InternalServerError, ErrorMessages.NasaResponseWithErrors) : new(response);
        }
        #endregion
    }
}
