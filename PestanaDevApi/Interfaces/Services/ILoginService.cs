using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ILoginService
    {
        Task<LoginResponseDto> Login(LoginRequestDto request);
        Task<LoginResponseDto> LoginWithProvider(LoginWithPlatformRequestDto request);
    }
}
