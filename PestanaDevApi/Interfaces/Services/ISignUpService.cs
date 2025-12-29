using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ISignUpService
    {
        Task<ApiToken?> SignUp(SignUpRequestDto request);
    }
}
