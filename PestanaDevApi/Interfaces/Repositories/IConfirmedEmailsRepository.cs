namespace PestanaDevApi.Interfaces.Repositories
{
    public interface IConfirmedEmailsRepository
    {
        Task InsertCofirmedEmail(string email);
        Task<bool> IsEmailConfirmed(string email);
    }
}
