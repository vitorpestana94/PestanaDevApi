using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Models;

namespace PestanaDevApi.Extensions.Dtos.Requests
{
    public static class ChangeUserDataRequestDtoExtensions
    {
        public static object ToUpdate(this ChangeUserDataRequestDto dto, User currentUserData, Guid userId) =>
        new
        {
            Name = GetWasFieldUpdated(newField: dto.Name, oldField: currentUserData.UserName) ? dto.Name : currentUserData.UserName,
            Email = GetWasFieldUpdated(newField: dto.Email, oldField: currentUserData.UserEmail) ? dto.Email : currentUserData.UserEmail,
            UserId = userId
        };

        public static bool UserEmailWasUpdated(this ChangeUserDataRequestDto dto, string currentUserEmail) => GetWasFieldUpdated(dto.Email, currentUserEmail);

        public static bool WasDataNotUpdated(this ChangeUserDataRequestDto dto) => string.IsNullOrEmpty(dto.Email) && string.IsNullOrEmpty(dto.Name);

        private static bool GetWasFieldUpdated(string? newField, string oldField)
        {
            if (string.IsNullOrEmpty(newField)) return false;

            return !string.Equals(newField, oldField, StringComparison.OrdinalIgnoreCase);
        }
    }
}
