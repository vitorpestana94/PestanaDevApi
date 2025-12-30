using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ISignUpRepository
    {
        Task<User> RegisterUser(User user);
        Task<bool> IsEmailBeingUsed(string email);
        Task<User> RegisterUserByPlatform(User user);
    }
}
