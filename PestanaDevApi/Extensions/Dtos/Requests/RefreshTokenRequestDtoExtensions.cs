using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class RefreshTokenRequestDtoExtensions
    {
        public static object ToSelectByRefreshToken(this RefreshTokenRequestDto dto) =>
        new
        {
            UserId = dto.UserId.ToString(),
            dto.Token,
            dto.DeviceId,
            DateTime.UtcNow
        };
    }
}
