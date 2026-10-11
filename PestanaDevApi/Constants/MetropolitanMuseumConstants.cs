namespace PestanaDevApi.Constants
{
    public static class MetropolitanMuseumConstants
    {
        public const string DefaultSearchTerm = "Van Gogh";
        // The v1 search was retired by the Met on 2026-10-01 (HTTP 410 Gone); v1.1 returns the same { total, objectIDs }.
        public const string SearchPath = "/v1.1/search";
        public const string ObjectsPath = "/v1/objects/";
    }
}
