using System.Text.Json.Serialization;

namespace PestanaDevApi.Dtos.Responses
{
    public class GitHubResponseEmailDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("primary")]
        public bool Primary { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }
}
