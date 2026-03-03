namespace PestanaDevApi.Interfaces.Services
{
    public interface IRequestService
    {
        Task<TResponse?> GetAsync<TResponse>(string endpoint, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    }
}
