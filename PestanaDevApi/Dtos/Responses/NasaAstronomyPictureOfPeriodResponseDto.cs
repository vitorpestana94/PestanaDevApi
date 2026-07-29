using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class NasaAstronomyPictureOfPeriodResponseDto: DefaultResponseDto
    {
        public IEnumerable<NasaResponse> NasaResponse { get; set; } = [];

        public NasaAstronomyPictureOfPeriodResponseDto()
        {
        }

        public NasaAstronomyPictureOfPeriodResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public NasaAstronomyPictureOfPeriodResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }

        public NasaAstronomyPictureOfPeriodResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
