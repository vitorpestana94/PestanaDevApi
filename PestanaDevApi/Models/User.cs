using Google.Apis.Auth;
using Newtonsoft.Json.Linq;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace PestanaDevApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string? UserPicture { get; set; }
        public string? UserPlatformId { get; set; }
        public Platform? UserSignUpPlatform { get; set; }

        public User() 
        {
            UserName = string.Empty;
            UserEmail = string.Empty;
            UserPassword = string.Empty;
            UserPicture = string.Empty;
        }

        public User(SignUpRequestDto dto)
        {
            UserName = dto.Name;
            UserEmail = dto.Email;
            UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            UserPicture = dto.Picture;
        }

        public User(GoogleJsonWebSignature.Payload googlePayload)
        {
            UserName = googlePayload.Name;
            UserEmail = googlePayload.Email;
            UserPassword = "";
            UserPicture = googlePayload.Picture;
            UserSignUpPlatform = Platform.Google;
            UserPlatformId = googlePayload.Subject;
        }

        public User(GoogleJsonWebSignature.Payload googlePayload, Guid userId)
        {
            Id = userId;
            UserName = googlePayload.Name;
            UserEmail = googlePayload.Email;
            UserPassword = "";
            UserPicture = googlePayload.Picture;
            UserSignUpPlatform = Platform.Google;
        }

        public User(GithubResponseDto responseDto, string userEmail)
        {
            UserName = responseDto.Username;
            UserEmail = userEmail;
            UserPassword = "";
            UserPicture = responseDto.AvatarUrl;
            UserSignUpPlatform = Platform.GitHub;
            UserPlatformId = responseDto.Id.ToString();
        }

        public User(GithubResponseDto responseDto, Guid userId, string userEmail)
        {
            Id= userId;
            UserName = responseDto.Username;
            UserEmail = userEmail;
            UserPassword = "";
            UserPicture = responseDto.AvatarUrl;
            UserSignUpPlatform = Platform.GitHub;
        }

        public User(JwtSecurityToken jwt, Guid userId, string userEmail)
        {
            Id = userId;
            UserName = jwt.Claims.First(c => c.Type == "name").Value;
            UserEmail = userEmail;
            UserPassword = "";
            UserPicture = jwt.Claims.First(c => c.Type == "picture").Value;
            UserSignUpPlatform = Platform.Linkedin;
        }

        public User(JwtSecurityToken jwt, string userId, string userEmail)
        {
            UserName = jwt.Claims.First(c => c.Type == "name").Value; ;
            UserEmail = userEmail;
            UserPassword = "";
            UserPicture = jwt.Claims.First(c => c.Type == "picture").Value;
            UserSignUpPlatform = Platform.Linkedin;
            UserPlatformId = userId;
        }
    }
}
