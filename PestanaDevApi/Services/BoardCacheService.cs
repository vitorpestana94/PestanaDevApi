using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;
using CacheKeys = PestanaDevApi.Constants.CacheKeysConstants;

namespace PestanaDevApi.Services
{
    public class BoardCacheService: IBoardCacheService
    {
        private readonly ICacheService _cacheService;

        // Board data contains user names, which can be changed outside the board flow,
        // so it expires sooner than the default 24 hours.
        private readonly TimeSpan _expiration = TimeSpan.FromMinutes(30);

        public BoardCacheService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        #region Board Data
        public GetBoardDataResponseDto? GetBoardDataCache(Guid boardId)
        {
            return _cacheService.Get<GetBoardDataResponseDto>(GetKey(CacheKeys.BoardDataKey, boardId));
        }

        public void SetBoardDataCache(Guid boardId, GetBoardDataResponseDto response)
        {
            _cacheService.Set(GetKey(CacheKeys.BoardDataKey, boardId), response, _expiration);
        }

        public void DeleteBoardDataCache(Guid boardId)
        {
            _cacheService.Remove(GetKey(CacheKeys.BoardDataKey, boardId));
        }
        #endregion

        #region User Boards
        public GetBoardsResponseDto? GetUserBoardsCache(Guid userId)
        {
            return _cacheService.Get<GetBoardsResponseDto>(GetKey(CacheKeys.UserBoardsKey, userId));
        }

        public void SetUserBoardsCache(Guid userId, GetBoardsResponseDto response)
        {
            _cacheService.Set(GetKey(CacheKeys.UserBoardsKey, userId), response, _expiration);
        }

        public void DeleteUserBoardsCache(Guid userId)
        {
            _cacheService.Remove(GetKey(CacheKeys.UserBoardsKey, userId));
        }
        #endregion

        #region Board Members
        public GetBoardMembersResponseDto? GetBoardMembersCache(Guid boardId)
        {
            return _cacheService.Get<GetBoardMembersResponseDto>(GetKey(CacheKeys.BoardMembersKey, boardId));
        }

        public void SetBoardMembersCache(Guid boardId, GetBoardMembersResponseDto response)
        {
            _cacheService.Set(GetKey(CacheKeys.BoardMembersKey, boardId), response, _expiration);
        }

        public void DeleteBoardMembersCache(Guid boardId)
        {
            _cacheService.Remove(GetKey(CacheKeys.BoardMembersKey, boardId));
        }
        #endregion

        #region Private Methods
        private static string GetKey(string key, Guid id) => $"{key}:{id}";
        #endregion
    }
}
