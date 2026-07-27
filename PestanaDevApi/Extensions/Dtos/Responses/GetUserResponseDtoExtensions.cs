using Microsoft.AspNetCore.Mvc;
using PestanaDevApi.Dtos.Responses.Failures;
using PestanaDevApi.Dtos.Responses;
using System.Net;
using Org.BouncyCastle.Asn1.Ocsp;
using PestanaDevApi.Models.Enums;

namespace PestanaDevApi.Extensions.Dtos.Responses
{
    public static class GetUserResponseDtoExtensions
    {
        public static bool UserRegisteredUsingPlatform(this GetUserResponseDto dto)
        {
            return dto?.Data?.RegisterType == RegisterTypeEnum.Platform.ToString();
        }
    }
}
