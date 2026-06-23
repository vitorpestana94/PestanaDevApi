using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Models;
using PestanaDevApi.Constants.Messages;
using System.Net;

namespace PestanaDevApi.Services
{
    public class RefreshTokenService: IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repo;
        private readonly ITokenService _tokenService;

        public RefreshTokenService(IRefreshTokenRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RefreshToken(RefreshTokenRequestDto request)
        {
            User? data = await _repo.GetUserDataByRefreshToken(request);

            if (data == null)
                return new AuthResponseDto(HttpStatusCode.Unauthorized, ErrorMessages.Unauthorized);

            string? refreshToken = await _tokenService.GetAndUpdateRefreshToken(request);

            if (string.IsNullOrEmpty(refreshToken))
                return new AuthResponseDto(HttpStatusCode.InternalServerError, ErrorMessages.InternalServerError);

            return new(_tokenService.GenerateApiTokensWithRefreshToken(data, refreshToken));
        }
    }
}
