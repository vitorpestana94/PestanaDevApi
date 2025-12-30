using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ILoginService
    {
        Task<ApiToken?> Login(LoginRequestDto request);
    }
}
