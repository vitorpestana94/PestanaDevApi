using PestanaDevApi.Dtos.Responses;

namespace PestanaDevApi.Interfaces.Services
{
    public interface IUserService
    {
        Task<GetUserResponseDto> GetUser(Guid userId);
    }
}
