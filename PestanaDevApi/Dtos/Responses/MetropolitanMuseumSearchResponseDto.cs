using System.Text.Json.Serialization;

public class MetropolitanMuseumSearchResponseDto
{
    [JsonPropertyName("objectID")]
    public int ObjectId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("primaryImage")]
    public string? PrimaryImage { get; set; }

    [JsonPropertyName("primaryImageSmall")]
    public string? PrimaryImageSmall { get; set; }

    [JsonPropertyName("artistDisplayName")]
    public string? ArtistDisplayName { get; set; }

    [JsonPropertyName("artistDisplayBio")]
    public string? ArtistDisplayBio { get; set; }

    [JsonPropertyName("objectDate")]
    public string? ObjectDate { get; set; }

    [JsonPropertyName("culture")]
    public string? Culture { get; set; }

    [JsonPropertyName("medium")]
    public string? Medium { get; set; }

    [JsonPropertyName("dimensions")]
    public string? Dimensions { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("isPublicDomain")]
    public bool IsPublicDomain { get; set; }

    [JsonPropertyName("objectURL")]
    public string? ObjectUrl { get; set; }
}