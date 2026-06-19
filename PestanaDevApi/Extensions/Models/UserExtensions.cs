using Google.Apis.Auth;
using PestanaDevApi.Dtos.Responses;
using System.IdentityModel.Tokens.Jwt;
using PestanaDevApi.Models;

namespace PestanaDevApi.Extensions.Models
{
    public static class UserExtensions
    {
        public static object ToInsert(this User user) =>
        new
        {
            Name = user.UserName,
            Email = user.UserEmail,
            Password = user.UserPassword
        };
    }
}
