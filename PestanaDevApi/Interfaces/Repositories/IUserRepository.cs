using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUser(Guid userId);
        Task<bool> UpdateUserData(ChangeUserDataRequestDto dto, User oldData, Guid userId);
        Task<bool> DeleteUserData(Guid userId);
    }
}
