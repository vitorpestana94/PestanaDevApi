using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ILoginService
    {
        Task<AuthResponseDto> Login(LoginRequestDto request);
        Task<AuthResponseDto> LoginOrSignUpWithProvider(LoginOrSignUpWithPlatformRequestDto request);
        Task LogoutUser(Guid userId, string deviceId);
    }
}
