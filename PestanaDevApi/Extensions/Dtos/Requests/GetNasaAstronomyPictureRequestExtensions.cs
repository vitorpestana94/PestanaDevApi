using System.Globalization;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class GetNasaAstronomyPictureRequestExtensions
    {
        public static bool IsSpecificDatePicture(this GetNasaAstronomyPictureOfDayRequest dto) => !string.IsNullOrEmpty(dto.Date);
        public static bool IsDateNotValid(this GetNasaAstronomyPictureOfDayRequest dto) => !ApiLib.CheckDate(dto.Date!) || ApiLib.IsFutureDate(dto.Date!);
    }
}
