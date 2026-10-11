using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces.Services.Email
{
    public interface IEmailTemplateService
    {
        Task<string> GetEmailTemplate(ContactEmailRequestDto request, bool isContactConfirmation = false);
        Task<string> GetEmailTemplate(ConfirmationCodeEmailRequestDto request, string confirmationCodes);

        /// <summary>
        /// Builds the email that invites a not registered user to sign up and join a board,
        /// translated to the request locale.
        /// </summary>
        /// <param name="request">
        /// Data of the invitation (invited email, board owner email and locale).
        /// </param>
        /// <param name="signUpUrl">
        /// The client side sign up url opened by the email button.
        /// </param>
        /// <returns>
        /// The email HTML.
        /// </returns>
        Task<string> GetEmailTemplate(BoardInvitationEmailDto request, string signUpUrl);

        /// <summary>
        /// Retrieves the board invitation email subject, translated to the request locale.
        /// </summary>
        /// <param name="request">
        /// Data of the invitation.
        /// </param>
        /// <returns>
        /// The email subject.
        /// </returns>
        string GetBoardInvitationSubject(BoardInvitationEmailDto request);
    }
}
