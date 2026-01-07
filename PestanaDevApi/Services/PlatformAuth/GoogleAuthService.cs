using Google.Apis.Auth;
using PestanaDevApi.Interfaces.Services.Auth;

namespace PestanaDevApi.Services.Auth
{
    public class GoogleAuthService: IGoogleAuthService
    {
        private readonly IConfiguration _config;
        private readonly string _googleClientId;

        public GoogleAuthService(IConfiguration config) 
        { 
            _config = config;

            if (string.IsNullOrEmpty(_config["google.clientid.web"]))
                throw new InvalidOperationException("Google client id not configured!");

            _googleClientId = _config["google.clientid.web"]!;
        }

        /// <summary>
        /// Validates a Google  and ensures it matches configured client IDs.
        /// </summary>
        /// <param name="idToken">The Google ID token to validate.</param>
        /// <returns>The payload of the validated Google ID token.</returns>
        public async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken)
        {
            try
            {
                return await GoogleJsonWebSignature.ValidateAsync(idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [_googleClientId]
                });
            }
            catch
            {
                return null;
            }
        }
    }
}
