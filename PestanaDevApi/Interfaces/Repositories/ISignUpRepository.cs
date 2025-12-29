using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ISignUpRepository
    {
        Task<User> InsertUser(User user);
    }
}
