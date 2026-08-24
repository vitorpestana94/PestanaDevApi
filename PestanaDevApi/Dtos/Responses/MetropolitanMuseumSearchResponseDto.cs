using System.Text.Json.Serialization;

public class MetropolitanMuseumSearchResponseDto
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("primaryImageSmall")]
    public string? PrimaryImageSmall { get; set; }

    [JsonPropertyName("objectDate")]
    public string? ObjectDate { get; set; }

    [JsonPropertyName("isPublicDomain")]
    public bool IsPublicDomain { get; set; }
}