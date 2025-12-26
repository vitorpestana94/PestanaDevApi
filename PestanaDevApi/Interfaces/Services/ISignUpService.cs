using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces.Services
{
    public interface ISignUpService
    {
        Task SignUp(SignUpRequestDto request);
    }
}
