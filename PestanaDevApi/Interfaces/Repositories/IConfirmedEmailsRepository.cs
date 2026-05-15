namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IConfirmedEmailsRepository
    {
        Task RegisterEmailConfirmation(string email);
        Task<bool> IsEmailConfirmed(string email);
        Task DeleteEmailConfirmation(string email);
    }
}
