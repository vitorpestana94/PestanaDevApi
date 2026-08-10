using System.Text.Json.Serialization;

public class ArtInstituteOfChicagoSearchResponseDto
{
    [JsonPropertyName("data")]
    public IEnumerable<ArtworkDataDto> Data { get; set; } = [];

    [JsonPropertyName("config")]
    public ApiConfig Config { get; set; } = new();
}

public class ArtworkDataDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("image_id")]
    public string? ImageId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("date_display")]
    public string? DateDisplay { get; set; }

    [JsonPropertyName("date_end")]
    public int? DateEnd { get; set; }

    [JsonPropertyName("artist_display")]
    public string? ArtistDisplay { get; set; }

    [JsonPropertyName("place_of_origin")]
    public string? PlaceOfOrigin { get; set; }

    [JsonPropertyName("medium_display")]
    public string? MediumDisplay { get; set; }

    [JsonPropertyName("publication_history")]
    public string? PublicationHistory { get; set; }

    [JsonPropertyName("exhibition_history")]
    public string? ExhibitionHistory { get; set; }

    [JsonPropertyName("artist_titles")]
    public string? ArtistTitles { get; set; }
}

public class ApiConfig
{
    [JsonPropertyName("iiif_url")]
    public string IiifUrl { get; set; } = string.Empty;
}