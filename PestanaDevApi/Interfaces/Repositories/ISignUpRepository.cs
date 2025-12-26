using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ISignUpRepository
    {
        Task InsertUser(User user);
    }
}
