using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Extensions.Dtos.Requests;
using PestanaDevApi.Constants.Messages;
using System.Net;
using PestanaDevApi.Utils;
using PestanaDevApi.Interfaces;

namespace PestanaDevApi.Services
{
    public class NasaIntegrationService: INasaIntegrationService
    {
        private readonly INasaRequestService _requestService;
        private readonly INasaCacheService _cacheService;

        public NasaIntegrationService(INasaRequestService requestService, INasaCacheService cacheService)
        {
            _requestService = requestService;
            _cacheService = cacheService;
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

            return await GetNasaAstronomyPictureOfThePeriod(request);
        }

        public async Task RefreshNasaCache()
        {
            (string startDate, string endDate) = DateLib.GetStartEndDate();

            _cacheService.DeleteNasaCache();

            await RequestNasa();
            await RequestNasa(startDate, endDate); 
        }

        #region Private Methods
        /// <summary>
        /// Retrieves the Astronomy Pictures for the requested period from the cache,
        /// or requests them from the NASA API if no cached data is available.
        /// </summary>
        /// <param name="request">
        /// Request containing the start and end dates of the desired period.
        /// </param>
        /// <returns>
        /// A response containing the Astronomy Pictures for the requested period,
        /// either from the cache or from the NASA API.
        /// </returns>
        private async Task<NasaAstronomyPicturesOfPeriodResponseDto> GetNasaAstronomyPictureOfThePeriod(GetNasaAstronomyPicturesOfPeriodRequest request)
        {
            NasaAstronomyPicturesOfPeriodResponseDto? cache = _cacheService.GetNasaAstronomyPictureOfThePeriodCache();

            if (cache != null)
            {
                if (request.IsCached(cache))
                    return cache;

                return await RequestNasa(request.StartDate!, request.EndDate!, shouldCache: false); // Cache it's not being done here because it's a unusual request
            }                                                                                      // since the unique client of this system should be the portfolio client.

            return await RequestNasa(request.StartDate!, request.EndDate!);
        }

        /// <summary>
        /// Retrieves today's Astronomy Picture of the Day from the NASA API.
        /// </summary>
        /// <returns>
        /// A response containing today's Astronomy Picture of the Day
        /// or an error response.
        /// </returns>
        private async Task<NasaAstronomyPictureOfDayResponseDto> GetTheAstronomyPictureOfToday()
        {
            return _cacheService.GetNasaAstronomyPictureOfTodayCache() ?? await RequestNasa();
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

        /// <summary>
        /// Requests the Astronomy Picture of the Day from the NASA API,
        /// stores the result in the cache, and returns the response.
        /// </summary>
        /// <returns>
        /// A response containing today's Astronomy Picture of the Day,
        /// or an error response if the NASA API request fails.
        /// </returns>
        private async Task<NasaAstronomyPictureOfDayResponseDto> RequestNasa()
        {
            NasaResponse? response = await _requestService.GetNasaPictureOfToday();

            if (response == null)
                return new(HttpStatusCode.InternalServerError, ErrorMessages.NasaResponseWithErrors);

            NasaAstronomyPictureOfDayResponseDto apiResponse = new(response);

            _cacheService.SetNasaAstronomyPictureOfTodayCache(apiResponse);

            return apiResponse;
        }

        /// <summary>
        /// Requests the Astronomy Pictures for the specified period from the NASA API,
        /// stores the result in the cache, and returns the response.
        /// </summary>
        /// <param name="request">
        /// Request containing the start and end dates of the desired period.
        /// </param>
        /// <returns>
        /// A response containing the Astronomy Pictures for the specified period,
        /// or an error response if the NASA API request fails.
        /// </returns>
        private async Task<NasaAstronomyPicturesOfPeriodResponseDto> RequestNasa(string startDate, string endDate, bool shouldCache = true)
        {
            IEnumerable<NasaResponse>? response = await _requestService.GetNasaPicturesOfPeriod(startDate, endDate);
            
            if (response == null)
                return new(HttpStatusCode.InternalServerError, ErrorMessages.NasaResponseWithErrors);

            NasaAstronomyPicturesOfPeriodResponseDto apiResponse = new(response);

            if (shouldCache)
                _cacheService.SetNasaAstronomyPictureOfThePeriodCache(apiResponse);

            return apiResponse;
        }
        #endregion
    }
}
