using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using PestanaDevApi.Interfaces.Services.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PestanaDevApi.Services.Auth
{
    public class LinkedinAuthService : ILinkedinAuthService
    {
        private readonly IConfiguration _config;
        private readonly string _linkedinPublicKeysUrl;
        private readonly string _linkedinValidIssUser;
        private readonly string _linkedinClientId;

        public LinkedinAuthService(IConfiguration config)
        {
            _config = config;

            if (string.IsNullOrEmpty(_config["linkedin.publickeysurl"]))
                throw new InvalidOperationException("Linkedin public key url not configured!");

            if (string.IsNullOrEmpty(_config["linkedin.validissuser"]))
                throw new InvalidOperationException("Linkedin iss user key url not configured!");

            if (string.IsNullOrEmpty(_config["linkedin.clientid"]))
                throw new InvalidOperationException("Linkedin client id not configured!");

            _linkedinPublicKeysUrl = _config["linkedin.publickeysurl"]!;
            _linkedinValidIssUser = _config["linkedin.validissuser"]!;
            _linkedinClientId = _config["linkedin.clientid"]!;
        }

        /// <summary>
        /// Validates the provided Linkedin's id_token. If valid, it can return an existing user via Linkedin's user email response 
        /// or create a new one if no user is found. 
        /// Otherwise, it will return null because the id_token is invalid.
        /// </summary>
        /// <param name="idToken">The Linkedin's acess_token.</param>
        /// <returns>The valid decodeded JWT token or null if it's invalid.</returns>
        public async Task<JwtSecurityToken?> ValidateLinkedinIdToken(string idToken)
        {
            JwtSecurityTokenHandler jwtHandler = new();

            try
            {
                jwtHandler.ValidateToken(idToken, await GetTokenValidationParams(), out var validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch
            {
                return null;
            }
        }

        public string GetUserEmailFromJwt(JwtSecurityToken jwt)
        {
            return jwt.Claims.First(claims => claims.Type == "email").Value;
        }
        #region Private Methods
        /// <summary>
        /// Get Linkedin's id_token validation token parameters.
        /// </summary>
        /// <returns>Linkedin's id_token validation token parameters.</returns>
        private async Task<TokenValidationParameters> GetTokenValidationParams()
        {
            OpenIdConnectConfiguration config = await GetTokenValidationConfig();

            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _linkedinValidIssUser,
                ValidateAudience = true,
                ValidAudience = _linkedinClientId,
                ValidateLifetime = true,
                IssuerSigningKeys = config.SigningKeys,
                ValidateIssuerSigningKey = true
            };
        }

        /// <summary>
        /// Get Linkedin's id_token validation token validation configs.
        /// </summary>
        /// <returns>Linkedin's id_token validation token validation configs.</returns>
        private async Task<OpenIdConnectConfiguration> GetTokenValidationConfig()
        {
            ConfigurationManager<OpenIdConnectConfiguration> configurationManager = new(_linkedinPublicKeysUrl, new OpenIdConnectConfigurationRetriever());

            return await configurationManager.GetConfigurationAsync();
        }
        #endregion
    }
}
