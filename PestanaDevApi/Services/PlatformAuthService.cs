using Google.Apis.Auth;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;

namespace PestanaDevApi.Services
{
    public class PlatformAuthService : IPlatformAuthService
    {
        private readonly IConfiguration _config;
        private readonly ILoginRepository _loginRepository;
        private readonly ISignUpRepository _signUpRepository;

        public PlatformAuthService(IConfiguration configuration, ILoginRepository loginRepository, ISignUpRepository signUpRepository) 
        {
            _config = configuration;
            _loginRepository = loginRepository;
            _signUpRepository = signUpRepository;

            if (string.IsNullOrEmpty(_config["google.clientid.web"]))
                throw new InvalidOperationException("Google client id not configured!");
        }

        /// <summary>
        /// Validates the provided idToken.If valid, it can return an existing user via Google's payload email 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the provided ID token is invalid.
        /// </summary>
        /// <param name="idToken">The Google's ID toke.</param>
        /// <returns>Existing user data or create new data after registering.</returns>
        public async Task<User?> HandleGoogleIdToken(string idToken)
        {
            GoogleJsonWebSignature.Payload? googlePayload = await ValidateGoogleToken(idToken);

            if (googlePayload == null)
                return null;

            return await _loginRepository.GetUserDataByEmail(googlePayload.Email) ?? await _signUpRepository.RegisterUserByPlatform(new User(googlePayload));
        } 

        /// <summary>
        /// Validates a Google  and ensures it matches configured client IDs.
        /// </summary>
        /// <param name="idToken">The Google ID token to validate.</param>
        /// <returns>The payload of the validated Google ID token.</returns>
        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken)
        {
            try
            {
             return await GoogleJsonWebSignature.ValidateAsync(idToken,
             new GoogleJsonWebSignature.ValidationSettings
             {
                 Audience = [_config["google.clientid.web"]!]
             });
            }
            catch
            {
                return null;
            }
        }
    }
}
