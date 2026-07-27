using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IUserService
    {
        Task<GetUserResponseDto> GetUser(Guid userId);
        Task<GetUserResponseDto> GetUserByEmail(string userEmail);
        Task<ChangeUserDataResponseDto> ChangeUserData(ChangeUserDataRequestDto dto, Guid userId);
        Task<DeleteUserResponseDto> DeleteUser(Guid userId);
        Task<ChangePasswordResponseDto> ChangeUserPassword(ChangePasswordRequestDto dto, Guid userId);
    }
}
