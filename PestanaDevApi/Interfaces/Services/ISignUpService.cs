using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ISignUpService
    {
        Task<SignUpResponseDto> SignUp(SignUpRequestDto request);
        Task<IsEmailAlreadyRegisteredResponseDto> IsEmailAlreadyRegistered(string email);
    }
}
