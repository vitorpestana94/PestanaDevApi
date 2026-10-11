namespace PestanaDevApi.Constants
{
    /// <summary>
    /// Paths and query parameters of the client side system (front end).
    /// </summary>
    public static class ClientSideConstants
    {
        public const string ClientSideSystemUrlConfigKey = "clientSideSystemUrl";
        public const string SignUpPath = "authentication/sign-up";

        #region Query Parameters
        public const string RedirectToQueryKey = "redirectTo";
        public const string ScrumBoardRedirect = "scrum-board";
        public const string BoardIdQueryKey = "boardId";
        #endregion
    }
}
