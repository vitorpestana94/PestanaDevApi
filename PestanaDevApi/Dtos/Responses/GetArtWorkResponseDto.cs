using System.Net;


namespace PestanaDevApi.Dtos.Responses
{
    public class GetArtWorkResponseDto: DefaultResponseDto
    {
        public IEnumerable<MetropolitanMuseumSearchResponseDto> ArtData { get; set; } = [];

        public GetArtWorkResponseDto()
        {
        }

        public GetArtWorkResponseDto(MetropolitanMuseumSearchResponseDto[] response) : base()
        {
            ArtData = response.Where(art => art.IsPublicDomain && !string.IsNullOrEmpty(art.PrimaryImageSmall));
        }

        public GetArtWorkResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public GetArtWorkResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public GetArtWorkResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }
    }
}
