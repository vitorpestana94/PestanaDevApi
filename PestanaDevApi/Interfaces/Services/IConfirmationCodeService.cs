using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IConfirmationCodeService
    {
        Task<string> GenerateConfirmationCode(string userEmail, bool isResend = false);

        /// <summary>
        /// Checks whether there is at least one confirmation code record associated with the given email.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>
        /// <c>true</c> if a record exists for the specified email; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method executes a query that returns a constant value (1) if a matching record exists.
        /// It is intended to be used as an existence check rather than retrieving full entity data.
        /// </remarks>
        Task<bool> CheckIfConfirmationCodeEmailAlreadySent(string email);

        /// <summary>
        /// Validates whether the provided confirmation code is valid for the given request.
        /// </summary>
        /// <param name="request">
        /// The request containing the client's email and the confirmation code to be validated.
        /// </param>
        /// <returns>
        /// A <see cref="CheckConfirmationCodeResponse"/> indicating the result of the validation,
        /// including appropriate HTTP status codes and error messages when applicable.
        /// </returns>
        /// <remarks>
        /// The validation flow consists of the following steps:
        /// <list type="number">
        /// <item>
        /// Verifies whether the email exists in the data store.
        /// </item>
        /// <item>
        /// Checks whether the confirmation code associated with the email is still valid (not expired).
        /// </item>
        /// <item>
        /// Validates whether the provided code matches the stored confirmation code.
        /// </item>
        /// <item>
        /// If all validations pass, deletes the confirmation code to prevent reuse.
        /// </item>
        /// </list>
        /// 
        /// Returns a BadRequest response in the following scenarios:
        /// - Email does not exist.
        /// - Confirmation code is expired.
        /// - Confirmation code is invalid.
        /// 
        /// If validation succeeds, returns a successful response with default values.
        /// </remarks>
        Task<CheckConfirmationCodeResponse> IsConfirmationCodeValid(CheckConfirmationCodeRequestDto requestl);

        /// <summary>
        /// Deletes all expired (no longer valid) confirmation codes from the database.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous cleanup operation.
        /// </returns>
        /// <remarks>
        /// Executes a batch delete operation to remove all confirmation codes
        /// that are outside their valid time window.
        /// Intended for periodic cleanup (e.g., via scheduled job).
        /// </remarks>
        Task DeleteUnfreshConfirmationCodes();

        Task<bool> CheckCreatedAt(string email);

        /// <summary>
        /// Checks whether a confirmation code email has already been sent to the provided email address.
        /// </summary>
        /// <param name="email">The email address to be verified.</param>
        /// <returns>
        /// A <see cref="CheckIfConfirmationCodeEmailAlreadySentResponseDto"/> containing the validation result
        /// and information indicating whether a confirmation code email has already been sent.
        /// </returns>
        /// <remarks>
        /// The validation flow consists of the following steps:
        /// <list type="number">
        /// <item>
        /// Validates whether the provided email has a valid format.
        /// </item>
        /// <item>
        /// Checks whether there is already an active confirmation code associated with the email.
        /// </item>
        /// </list>
        /// 
        /// Returns a BadRequest response when the provided email format is invalid.
        /// 
        /// If the email is valid, returns a response indicating whether a confirmation code
        /// email has already been sent.
        /// </remarks>
        Task<CheckIfConfirmationCodeEmailAlreadySentResponseDto> CheckConfirmationCodeEmailAlreadySent(string email);
    }
}
