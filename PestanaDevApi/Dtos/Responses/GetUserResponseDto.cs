using PestanaDevApi.Models;
using PestanaDevApi.Models.Enums;
using System.Net;


namespace PestanaDevApi.Dtos.Responses
{
    public class GetUserResponseDto: DefaultResponseDto
    {
        public UserData? Data { get; set; } = null;

        public GetUserResponseDto()
        {
        }

        public GetUserResponseDto(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public GetUserResponseDto(User user) : base()
        {
            Data = new(user);
        }

        public GetUserResponseDto(string errorMessage) : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        public GetUserResponseDto(HttpStatusCode statusCode, string errorMessage) : base(statusCode, errorMessage) 
        { 
        }
    }

    public class UserData
    {
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string RegisterType { get; set; } = string.Empty;

        public UserData(User user)
        {
            Name = user.UserName;
            Email = user.UserEmail;
            RegisterType = user.SignupByPlatform ? RegisterTypeEnum.Platform.ToString() : RegisterTypeEnum.Manual.ToString();
        }
    }
}
