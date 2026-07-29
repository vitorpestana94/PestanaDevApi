namespace PestanaDevApi.Interfaces.Services
{
    public interface IRequestService
    {
        Task<TResponse?> GetAsync<TResponse>(string endpoint, Dictionary<string, string>? headers = null, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default);
    }
}
