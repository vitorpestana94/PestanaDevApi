namespace PestanaDevApi.Constants
{
    public static class QuartzConstants
    {
        public const string SchedulerName = "MyScheduler";
        public const string SchedulerId = "AUTO";

        #region Cron Expressions
        public const string DeleteUnfreshConfirmationCodesJob = "0 */15 * * * ?";
        public const string DeleteExpiredRefreshTokensJob = "0 0 3 * * ?";
        #endregion
    }
}
