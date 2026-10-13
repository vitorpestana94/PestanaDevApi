namespace PestanaDevApi.Constants
{
    public static class TokenConstants
    {
        #region Main Token
        public const string ApiAudience = "PestanaDevApi";
        public const int ApiTokenExpirationMinutes = 60;
        #endregion

        #region Hub Token
        /// <summary>
        /// Authentication scheme that only accepts hub tokens (used by the SignalR hubs).
        /// </summary>
        public const string HubTokenScheme = "HubToken";
        public const string HubAudience = "PestanaDevApi.Hub";
        public const int HubTokenExpirationMinutes = 10;
        #endregion
    }
}
