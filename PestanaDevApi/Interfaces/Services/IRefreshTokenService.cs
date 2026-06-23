using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        Task<AuthResponseDto> RefreshToken(RefreshTokenRequestDto request);
    }
}
