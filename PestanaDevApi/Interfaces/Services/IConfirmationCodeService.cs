using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IConfirmationCodeService
    {
        Task<string> GenerateConfirmationCode(string userEmail);

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
        Task<bool> CheckIfConfirmationCodeEmailAlreadySended(string email);

        Task<CheckConfirmationCodeResponse> IsConfirmationCodeValid(CheckConfirmationCodeRequest requestl);
    }
}
