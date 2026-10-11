using Microsoft.AspNetCore.WebUtilities;
using Consts = PestanaDevApi.Constants.ClientSideConstants;
using Locale = PestanaDevApi.Constants.LocaleConstants;

namespace PestanaDevApi.Utils
{
    public static class ClientSideUrlBuilder
    {
        /// <summary>
        /// Builds the client side sign up url used on the board invitation email.
        /// e.g. http://localhost:3000/pt/authentication/sign-up?redirectTo=scrum-board&amp;boardId={boardId}
        /// </summary>
        /// <param name="clientSideSystemUrl">
        /// The client side system base url.
        /// </param>
        /// <param name="locale">
        /// The client locale. Unknown locales use the default one (english).
        /// </param>
        /// <param name="boardId">
        /// Identifier of the board the user was invited to.
        /// </param>
        /// <returns>
        /// The sign up url with the "redirectTo" and "boardId" query parameters, in this order.
        /// </returns>
        public static string GetBoardInvitationSignUpUrl(string clientSideSystemUrl, string locale, Guid boardId)
        {
            string urlLocale = locale == Locale.Pt ? Locale.Pt : Locale.Default;
            string signUpUrl = $"{clientSideSystemUrl.TrimEnd('/')}/{urlLocale}/{Consts.SignUpPath}";

            signUpUrl = QueryHelpers.AddQueryString(signUpUrl, Consts.RedirectToQueryKey, Consts.ScrumBoardRedirect);

            return QueryHelpers.AddQueryString(signUpUrl, Consts.BoardIdQueryKey, boardId.ToString());
        }
    }
}
