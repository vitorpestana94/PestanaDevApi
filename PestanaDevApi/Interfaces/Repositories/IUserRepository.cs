using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUser(Guid userId);
    }
}
