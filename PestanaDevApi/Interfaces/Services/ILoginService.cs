using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ILoginService
    {
        Task<AuthResponseDto> Login(LoginRequestDto request);
        Task<AuthResponseDto> LoginOrSignUpWithProvider(LoginOrSignUpWithPlatformRequestDto request);
        Task LogoutUser(Guid userId, string deviceId);

        /// <summary>
        /// Generates the token used to connect to the SignalR hubs. The client must request a new one
        /// on every connection and reconnection, since it expires in 10 minutes.
        /// </summary>
        /// <param name="userId">
        /// Identifier of the authenticated user (from the main JWT).
        /// </param>
        /// <returns>
        /// A response containing the hub token.
        /// </returns>
        HubTokenResponseDto GenerateHubToken(Guid userId);
    }
}
