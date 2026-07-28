using PestanaDevApi.Models;
using System.Net;

namespace PestanaDevApi.Dtos.Responses
{
    public class NasaAstronomyPictureOfTheDayResponseDto: DefaultResponseDto
    {
        public string Copyright { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public string Titlte { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public NasaAstronomyPictureOfTheDayResponseDto()
        {
        }

        public NasaAstronomyPictureOfTheDayResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public NasaAstronomyPictureOfTheDayResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage)
        {
        }

        public NasaAstronomyPictureOfTheDayResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }
    }
}
