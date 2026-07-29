using System.Globalization;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class GetNasaAstronomyPictureOfPeriodRequestExtensions
    {

        public static bool CheckStartAndEndDates(this GetNasaAstronomyPictureOfPeriodRequest dto)
        {
            if (!AreDatesValid(dto.StartDate, dto.EndDate) ||
                !AreDatesConsistent(dto.StartDate, dto.EndDate) ||
                IsEndDateBeforeStartDate(dto.StartDate!, dto.EndDate!))
                return false;

            return true;
        }

        #region Private Methods
        private static bool AreDatesValid(string? startDate, string? endDate)
        {
            bool isValid = true;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                foreach (string date in new string[] { startDate, endDate })
                {
                    isValid = ApiLib.CheckDate(date);

                    if (!isValid)
                        return false;
                }
            }

            return isValid;
        }

        private static bool AreDatesConsistent(string? startDate, string? endDate) => string.IsNullOrEmpty(startDate) == string.IsNullOrEmpty(endDate);

        private static bool IsEndDateBeforeStartDate(string startDate, string endDate)
        {
            DateTime startDateAsDate = ParseDate(startDate);
            DateTime endDateAsDate = ParseDate(endDate);

            return startDateAsDate > endDateAsDate;
        }

        private static DateTime ParseDate(string date) => DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
        #endregion
    }
}
