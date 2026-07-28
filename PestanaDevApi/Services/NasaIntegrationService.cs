using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Extensions.Dtos.Requests;

namespace PestanaDevApi.Services
{
    public class NasaIntegrationService: INasaIntegrationService
    {
        public async Task<NasaAstronomyPictureOfTheDayResponseDto> GetNasaAstronomyPicture(GetNasaAstronomyPictureRequest request)
        {
            if (request.IsSpecificDatePicture())
                return await GetTheAstronomyPictureOfTheDay(request.Date!);


            if (request.IsPicturesFromPeriod())
                return await GetTheAstronomyPicturePeriod(request.StartDate!, request.EndDate!);

            return await GetTheAstronomyPictureOfToday();
        }

        #region Private Methods
        private async Task<NasaAstronomyPictureOfTheDayResponseDto> GetTheAstronomyPictureOfToday()
        {
        }

        private async Task<NasaAstronomyPictureOfTheDayResponseDto> GetTheAstronomyPicturePeriod(GetNasaAstronomyPictureRequest request)
        {
            if (request.CheckStartAndEndDates())
                return // retornar aqui a dto de erro.
        }

        private async Task<NasaAstronomyPictureOfTheDayResponseDto> GetTheAstronomyPictureOfTheDay(GetNasaAstronomyPictureRequest request)
        {

            if (request.CheckSpecificDate())
                return // retornar aqui a dto de erro.
        }
        #endregion
    }
}
