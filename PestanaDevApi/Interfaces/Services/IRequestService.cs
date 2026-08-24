namespace PestanaDevApi.Interfaces.Services
{
    public interface IRequestService
    {
        Task<TResponse?> RequestAsync<TResponse>(string endpoint, Dictionary<string, string>? headers = null, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default, HttpMethod? method = null);
    }
}
