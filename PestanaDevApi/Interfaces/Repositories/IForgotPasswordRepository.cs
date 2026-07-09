using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IForgotPasswordRepository
    {
        Task ChangeUserPassword(string email, string password);
    }
}
