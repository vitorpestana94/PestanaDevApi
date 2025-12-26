using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string? UserPicture { get; set; }

        public User() 
        {
            UserName = string.Empty;
            UserEmail = string.Empty;
            UserPassword = string.Empty;
            UserPicture = string.Empty;
        }

        public User(SignUpRequestDto dto)
        {
            UserName = dto.Nome;
            UserEmail = ApiLib.IsEmailValid(dto.Email);
            UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            UserPicture = dto.Picture;
        }
    }
}
