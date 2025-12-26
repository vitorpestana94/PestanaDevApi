namespace PestanaDevApi.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        Task InsertOrUpdateRefreshToken(Guid userId, string deviceId, string refreshToken);
    }
}
