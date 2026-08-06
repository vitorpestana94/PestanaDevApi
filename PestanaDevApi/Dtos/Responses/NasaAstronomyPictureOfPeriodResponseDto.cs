using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class NasaAstronomyPicturesOfPeriodResponseDto: DefaultResponseDto
    {
        public IEnumerable<NasaResponse>? NasaResponse { get; set; } = null;

        public NasaAstronomyPicturesOfPeriodResponseDto()
        {
        }

        public NasaAstronomyPicturesOfPeriodResponseDto(IEnumerable<NasaResponse> nasaResponse) : base()
        {
            NasaResponse = nasaResponse.OrderByDescending(response => response.Date) ;
        }

        public NasaAstronomyPicturesOfPeriodResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public NasaAstronomyPicturesOfPeriodResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }

        public NasaAstronomyPicturesOfPeriodResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
