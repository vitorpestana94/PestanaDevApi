using System.Globalization;

namespace PestanaDevApi.Utils
{
    public static class DateLib
    {
        private readonly static string _dateFormat = "yyyy-MM-dd";
        private readonly static CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private readonly static DateTimeStyles _dateTimeStyles = DateTimeStyles.None;

        public static bool CheckDate(string date) => DateTime.TryParseExact(date, _dateFormat, _cultureInfo, _dateTimeStyles, out DateTime _);

        public static DateTime ParseDate(string date) => DateTime.ParseExact(date, _dateFormat, _cultureInfo, _dateTimeStyles);

        public static bool IsFutureDate(string date) => ParseDate(date).Date > DateTime.UtcNow.Date;

        public static (string startDate, string endDate) GetStartEndDate()
        {
            DateTime yesterDay = DateTime.UtcNow.AddDays(-1).Date;
            DateTime oneWeekAgo = DateTime.UtcNow.AddDays(-7).Date;

            return (startDate: oneWeekAgo.ToString(_dateFormat), endDate: yesterDay.ToString(_dateFormat));
        }
    }
}
