using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ISignUpRepository
    {
        Task<Guid> InsertUser(User user);
    }
}
