using System.Globalization;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Exceptions;
using PestanaDevApi.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class GetNasaAstronomyPictureRequestExtensions
    {
        public static bool IsSpecificDatePicture(this GetNasaAstronomyPictureOfDayRequest dto) => !string.IsNullOrEmpty(dto.Date);
        public static bool CheckSpecificDate(this GetNasaAstronomyPictureOfDayRequest dto) => ApiLib.CheckDate(dto.Date!);
    }
}
