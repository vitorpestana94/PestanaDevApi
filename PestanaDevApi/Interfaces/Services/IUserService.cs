using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IUserService
    {
        Task<GetUserResponseDto> GetUser(Guid userId);
        Task<ChangeUserDataResponseDto> ChangeUserData(ChangeUserDataRequestDto dto, Guid userId);
        Task<DeleteUserResponseDto> DeleteUser(DeleteUserRequestDto dto, Guid userId);
        Task<ChangePasswordResponseDto> ChangeUserPassword(ChangePasswordRequestDto dto, Guid userId);
    }
}
