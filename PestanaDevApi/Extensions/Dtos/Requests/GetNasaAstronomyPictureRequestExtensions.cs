using System.Globalization;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Exceptions;
using PestanaDevApi.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class GetNasaAstronomyPictureRequestExtensions
    {
        public static bool IsSpecificDatePicture(this GetNasaAstronomyPictureRequest dto) => !string.IsNullOrEmpty(dto.Date);
        public static bool IsPicturesFromPeriod(this GetNasaAstronomyPictureRequest dto) => !string.IsNullOrEmpty(dto.StartDate) && !string.IsNullOrEmpty(dto.EndDate);
        public static bool CheckSpecificDate(this GetNasaAstronomyPictureRequest dto) => CheckDate(dto.Date!);

        public static bool CheckStartAndEndDates(this GetNasaAstronomyPictureRequest dto)
        {
            if (!AreDatesConsistent(dto.StartDate, dto.EndDate) || 
                !AreDatesValid(dto.StartDate, dto.EndDate) ||
                IsEndDateBeforeStartDate(dto.StartDate!, dto.EndDate!))
                return false;

            return true;
        }

        #region Private Methods
        private static bool CheckDate(string date) => DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d);
        private static bool AreDatesValid(string? startDate, string? endDate)
        {
            bool isValid = true;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                foreach (string date in new string[] { startDate, endDate })
                {
                    isValid = CheckDate(date);

                    if (!isValid)
                        return false;
                }
            }

            return isValid;
        }
        
        private static bool AreDatesConsistent(string? startDate, string? endDate) => string.IsNullOrEmpty(startDate) == string.IsNullOrEmpty(endDate);
        
        private static bool IsEndDateBeforeStartDate(string startDate, string endDate)
        {
            DateTime startDateAsDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime endDateAsDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            return startDateAsDate > endDateAsDate;
        }
        #endregion
    }
}
