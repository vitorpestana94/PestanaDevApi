using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IPlatformService
    {
        Task<AuthResponseDto> LoginOrSignUpWithProvider(LoginOrSignUpWithPlatformRequestDto request);
    }
}
