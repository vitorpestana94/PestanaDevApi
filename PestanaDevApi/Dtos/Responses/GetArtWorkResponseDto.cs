using System.Linq;
using System.Net;


namespace PestanaDevApi.Dtos.Responses
{
    public class GetArtWorkResponseDto: DefaultResponseDto
    {
        public IEnumerable<ArtData> ArtData { get; set; } = [];

        public GetArtWorkResponseDto()
        {
        }

        public GetArtWorkResponseDto(ArtInstituteOfChicagoSearchResponseDto response) : base()
        {
            ArtData = response.Data.Select(element => new ArtData(element, response.Config.IiifUrl));
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

    public class ArtData
    {
        public string ArtUrl { get; set; } = string.Empty;

        public ArtworkDataDto? Data { get; set; } = null;

        public ArtData() { }

        public ArtData(ArtworkDataDto data, string artUrl) : base()
        {
            ArtUrl = $"{artUrl}/{data.ImageId}/full/843,/0/default.jpg";
            Data = data;
        }
    }
}
