using System.Text.Json.Serialization;

namespace PestanaDevApi.Dtos.Responses
{
    public class GithubResponseDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("login")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        public GithubResponseDto() { }

        public GithubResponseDto(GithubResponseDto userResponse, GitHubResponseEmailDto emailResponse)
        {
            Id = userResponse.Id;
            Username = userResponse.Username;
            Email = emailResponse.Email;
            AvatarUrl = userResponse.AvatarUrl;
        }
    }
}
