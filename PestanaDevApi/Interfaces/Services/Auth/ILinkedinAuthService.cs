using System.IdentityModel.Tokens.Jwt;

namespace PestanaDevApi.Interfaces.Services.Auth
{
    public interface ILinkedinAuthService
    {
        Task<JwtSecurityToken?> ValidateLinkedinIdToken(string idToken);
        string GetUserEmailFromJwt(JwtSecurityToken jwt);
    }
}
