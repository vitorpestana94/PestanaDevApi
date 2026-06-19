using Google.Apis.Auth;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace PestanaDevApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public bool SignupByPlatform { get; set; }
        public string? UserPlatformId { get; set; }
        public PlatformEnum? UserSignUpPlatform { get; set; }

        public User() 
        {
            Id = Guid.Empty;
            UserName = string.Empty;
            UserEmail = string.Empty;
            UserPassword = string.Empty;
        }

        /// <summary>
        /// Creates an instance of a user who is not registered in the system
        /// based on data sended by the user on website form.
        /// </summary>
        public User(SignUpRequestDto dto)
        {
            UserName = dto.Name;
            UserEmail = dto.Email;
            UserPassword = HashFactory.HashPassword(dto.Password);
        }

        /// <summary>
        /// Creates an instance of a user who is not registered in the system
        /// based on data returned by Google.
        /// </summary>
        public User(GoogleJsonWebSignature.Payload googlePayload)
        {
            UserName = googlePayload.Name;
            UserEmail = googlePayload.Email;
            UserPassword = "";
            UserSignUpPlatform = PlatformEnum.Google;
            UserPlatformId = googlePayload.Subject;
        }

        /// <summary>
        /// Creates an instance of a user who is already registered in the system
        /// based on data returned by Google.
        /// </summary>
        public User(GoogleJsonWebSignature.Payload googlePayload, Guid userId)
        {
            Id = userId;
            UserName = googlePayload.Name;
            UserEmail = googlePayload.Email;
            UserPassword = "";
            UserSignUpPlatform = PlatformEnum.Google;
        }

        /// <summary>
        /// Creates an instance of a user who is not registered in the system
        /// based on data returned by GitHub.
        /// </summary>
        public User(GithubResponseDto responseDto, string userEmail)
        {
            UserName = responseDto.Username;
            UserEmail = userEmail;
            UserPassword = "";
            UserSignUpPlatform = PlatformEnum.GitHub;
            UserPlatformId = responseDto.Id.ToString();
        }

        /// <summary>
        /// Creates an instance of a user who is already registered in the system
        /// based on data returned by GitHub.
        /// </summary>
        public User(GithubResponseDto responseDto, Guid userId, string userEmail)
        {
            Id= userId;
            UserName = responseDto.Username;
            UserEmail = userEmail;
            UserPassword = "";
            UserSignUpPlatform = PlatformEnum.GitHub;
        }

        /// <summary>
        /// Creates an instance of a user who is already registered in the system
        /// based on data returned by Linkedin.
        /// </summary>
        public User(JwtSecurityToken jwt, Guid userId, string userEmail)
        {
            Id = userId;
            UserName = jwt.Claims.First(c => c.Type == "name").Value;
            UserEmail = userEmail;
            UserPassword = "";
            UserSignUpPlatform = PlatformEnum.Linkedin;
        }

        /// <summary>
        /// Creates an instance of a user who is not registered in the system
        /// based on data returned by Linkedin.
        /// </summary>
        public User(JwtSecurityToken jwt, string userId, string userEmail)
        {
            UserName = jwt.Claims.First(c => c.Type == "name").Value; ;
            UserEmail = userEmail;
            UserPassword = "";
            UserSignUpPlatform = PlatformEnum.Linkedin;
            UserPlatformId = userId;
        }
    }
}
