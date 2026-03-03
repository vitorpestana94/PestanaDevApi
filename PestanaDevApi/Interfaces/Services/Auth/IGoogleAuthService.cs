using Google.Apis.Auth;

namespace PestanaDevApi.Interfaces.Services.Auth
{
    public interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken);
    }
}
