using PestanaDevApi.Utils;
using PestanaDevApi.Dtos.Requests;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class ChangePasswordRequestDtoExtensions
    {
        public static object ToUpdate(this ChangePasswordRequestDto dto, Guid userId) =>
        new
        {
            Password = HashFactory.HashPassword(dto.NewPassword),
            UserId = userId
        };
    }
}
