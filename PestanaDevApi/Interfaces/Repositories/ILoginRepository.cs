using PestanaDevApi.Models;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ILoginRepository
    {
        Task<User?> GetUserDataByEmail(string email);
    }
}
