using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ISignUpService
    {
        Task<SignUpResponseDto> SignUp(SignUpRequestDto request);
    }
}
