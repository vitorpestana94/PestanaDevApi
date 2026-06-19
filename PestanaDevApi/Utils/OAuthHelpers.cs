using Google.Apis.Auth;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace PestanaDevApi.Utils
{
    public static class OAuthHelpers
    {
        /// <summary>
        /// Creates an instance of a user who is already registered in the system
        /// based on data returned by Google.
        /// </summary>
        public static User FromGoogleIdentity(GoogleJsonWebSignature.Payload googlePayload, Guid userId)
        {
            return new User(googlePayload, userId);
        }

        /// <summary>
        /// Creates an instance of a user who is already registered in the system
        /// based on data returned by GitHub.
        /// </summary>
        public static User FromGitHubIdentity(GithubResponseDto responseDto, Guid userId, string userEmail)
        {
            return new User(responseDto, userId, userEmail);
        }

        /// <summary>
        /// Creates an instance of a user who is already registered in the system
        /// based on data returned by Linkedin.
        /// </summary>
        public static User FromLinkedinIdentity(JwtSecurityToken jwt, Guid userId, string userEmail)
        {
            return new User(jwt, userId, userEmail);
        }
    }
}
