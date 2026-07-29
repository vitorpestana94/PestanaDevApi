using System.Globalization;
using PestanaDevApi.Dtos.Requests;
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

        #region Private Methods
        private static bool AreDatesValids(string? startDate, string? endDate)
        {
            foreach (string date in new string[] { startDate!, endDate! })
            {
                if (!ApiLib.CheckDate(date) || ApiLib.IsFutureDate(date))
                    return false;
            }

            return true;
        }

        private static bool AreDatesEmpty(string? startDate, string? endDate) => string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate);

        private static bool IsEndDateBeforeStartDate(string startDate, string endDate)
        {
            DateTime startDateAsDate = ApiLib.ParseDate(startDate);
            DateTime endDateAsDate = ApiLib.ParseDate(endDate);

            return startDateAsDate > endDateAsDate;
        }
        #endregion
    }
}
