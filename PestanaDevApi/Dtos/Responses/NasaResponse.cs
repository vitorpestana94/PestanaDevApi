using System.Text.Json.Serialization;

namespace PestanaDevApi.Dtos.Responses
{
    public class NasaResponse
    {
        public string Copyright { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; } = string.Empty;
    }
}
