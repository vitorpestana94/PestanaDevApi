using System.Text.Json.Serialization;

namespace PestanaDevApi.Dtos.Responses
{
    public class SearchArtWorksIdsResponseDto
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("objectIDs")]
        public IEnumerable<int> ObjectIds { get; set; } = [];
    }
}