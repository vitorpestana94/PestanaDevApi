using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUser(Guid userId);
        Task<User?> GetUserByEmail(string email);
        Task<bool> UpdateUserData(ChangeUserDataRequestDto dto, User oldData, Guid userId);
        Task<bool> DeleteUserData(Guid userId);
        Task<bool> ChangeUserPassword(ChangePasswordRequestDto dto, Guid userId);
    }
}
