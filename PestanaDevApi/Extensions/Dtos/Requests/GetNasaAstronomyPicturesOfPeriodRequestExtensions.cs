using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class GetNasaAstronomyPicturesOfPeriodRequestExtensions
    {

        public static bool AreBothDatesValids(this GetNasaAstronomyPicturesOfPeriodRequest dto)
        {
            if (AreDatesEmpty(dto.StartDate, dto.EndDate) ||
                !AreDatesValids(dto.StartDate, dto.EndDate) ||
                IsEndDateBeforeStartDate(dto.StartDate!, dto.EndDate!))
                return false;

            return true;
        }

        public static bool IsCached(this GetNasaAstronomyPicturesOfPeriodRequest dto, NasaAstronomyPicturesOfPeriodResponseDto cache)
        {
            if (cache.NasaResponse == null || !cache.NasaResponse.Any())
                return false;

            return cache.NasaResponse.Min(data => data.Date) == dto.StartDate && cache.NasaResponse.Max(data => data.Date) == dto.EndDate;
        }

        #region Private Methods
        private static bool AreDatesValids(string? startDate, string? endDate)
        {
            foreach (string date in new string[] { startDate!, endDate! })
            {
                if (!DateLib.CheckDate(date) || DateLib.IsFutureDate(date))
                    return false;
            }

            return true;
        }

        private static bool AreDatesEmpty(string? startDate, string? endDate) => string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate);

        private static bool IsEndDateBeforeStartDate(string startDate, string endDate)
        {
            DateTime startDateAsDate = DateLib.ParseDate(startDate);
            DateTime endDateAsDate = DateLib.ParseDate(endDate);

            return startDateAsDate > endDateAsDate;
        }
        #endregion
    }
}
