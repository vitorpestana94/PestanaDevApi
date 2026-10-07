namespace PestanaDevApi.Extensions
{
    public static class DateExtensions
    {
        public static string GetNasaDateFormat(this DateTime date)
        {
            return date.ToString("yyMMdd");
        }
    }
}
