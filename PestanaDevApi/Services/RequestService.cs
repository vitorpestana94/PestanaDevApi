using System.Text.Json;
using PestanaDevApi.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace PestanaDevApi.Services
{
    public class RequestService: IRequestService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public RequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse?> GetAsync<TResponse>(string endpoint, Dictionary<string, string>? headers = null, Dictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default)
        {
            if (queryParams != null)
                endpoint = QueryHelpers.AddQueryString(endpoint, queryParams!);

            using HttpRequestMessage request = new(HttpMethod.Get, endpoint);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }


            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "User-Agent",
                "curl/8.0.0"
            );

            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "Accept",
                "*/*"
            );

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.ReasonPhrase);

            string json = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
        }
    }
}
