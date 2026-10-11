using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task<EmailResponseDto> SendContactEmail(ContactEmailRequestDto request);
        Task<SendConfirmationCodeEmailResponseDto> SendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request);
        Task<SendConfirmationCodeEmailResponseDto> ResendConfirmationCodeEmail(ConfirmationCodeEmailRequestDto request);

        /// <summary>
        /// Sends the email that invites a not registered user to sign up and join a board.
        /// The email button opens the client side sign up page with the "redirectTo" and "boardId" query parameters.
        /// </summary>
        /// <param name="request">
        /// Data of the invitation (board id, invited email, board owner email and locale).
        /// </param>
        /// <exception cref="Exceptions.ApiException">
        /// Thrown if the client side system url is not configured or if the email can't be sent.
        /// </exception>
        Task SendBoardInvitationEmail(BoardInvitationEmailDto request);
    }
}
