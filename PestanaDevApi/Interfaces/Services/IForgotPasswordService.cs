using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IForgotPasswordService
    {
        Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request);
    }
}
