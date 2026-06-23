using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<User?> GetUserDataByRefreshToken(RefreshTokenRequestDto request);
    }
}
