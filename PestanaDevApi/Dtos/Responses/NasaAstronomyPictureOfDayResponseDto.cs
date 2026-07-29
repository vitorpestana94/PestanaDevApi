using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class NasaAstronomyPictureOfDayResponseDto: DefaultResponseDto
    {
        public NasaResponse? NasaResponse { get; set; } = null;

        public NasaAstronomyPictureOfDayResponseDto() 
        { 
        }

        public NasaAstronomyPictureOfDayResponseDto(NasaResponse response) : base()
        {
            NasaResponse = response;
        }

        public NasaAstronomyPictureOfDayResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public NasaAstronomyPictureOfDayResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }

        public NasaAstronomyPictureOfDayResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
