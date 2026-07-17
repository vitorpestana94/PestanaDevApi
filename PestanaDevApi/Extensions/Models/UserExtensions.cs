using PestanaDevApi.Models;
using PestanaDevApi.Constants;

namespace PestanaDevApi.Extensions.Models
{
    public static class UserExtensions
    {
        public static object ToInsert(this User user) =>
        new
        {
            Name = user.UserName,
            Email = user.UserEmail,
            Password = user.UserPassword,
            TermsVersion = WebPageVersionConstant.WebPageVersion
        };
    }
}
