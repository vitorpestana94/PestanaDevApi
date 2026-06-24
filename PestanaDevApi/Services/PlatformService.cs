using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using System.Net;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Interfaces.Services.Auth;

namespace PestanaDevApi.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformAuthService _platformAuthService;
        private readonly ITokenService _tokenService;

        public PlatformService(IPlatformAuthService platformAuthService, ITokenService tokenService) 
        { 
            _platformAuthService = platformAuthService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authenticates or registers a user via an external provider. 
        /// If the user does not exist in the local database, a new record is created (Sign-Up).
        /// If the user already exists, the session is initialized (Log-In).
        /// </summary>
        /// <param name="request">The request data containing the provider's token and a enum that indicates the provider.</param>
        /// <returns>A task representing the operation, yielding the authentication tokens.</returns>
        public async Task<AuthResponseDto> LoginOrSignUpWithProvider(LoginOrSignUpWithPlatformRequestDto request)
        {
            User? user = await _platformAuthService.GetUserByIoken(request.Token, request.Platform);

            if (user == null) // If the user is null, it means that the provided token is not valid for the requested platform.
                return new(HttpStatusCode.Forbidden);

            return new(await _tokenService.GenerateApiTokens(user));
        }
    }
}
