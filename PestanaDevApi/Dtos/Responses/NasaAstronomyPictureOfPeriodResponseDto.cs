using System.Net;
using PestanaDevApi.Extensions.Dtos.Responses;

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
            NasaResponse = nasaResponse.ParseNasaResponse();
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
