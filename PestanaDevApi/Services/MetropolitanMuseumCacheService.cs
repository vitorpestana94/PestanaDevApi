using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;

namespace PestanaDevApi.Services
{
    public class MetropolitanMuseumCacheService: IMetropolitanMuseumCacheService
    {
        private readonly ICacheService _cacheService;
        private readonly List<string> _cacheKeys = [];

        public MetropolitanMuseumCacheService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void DeleteMetropolitamMuseumCache()
        {
            foreach (string key in _cacheKeys)
            {
                _cacheService.Remove(key);
            }
        }

        public void SetMetropolitanMuseumCache(GetArtWorkResponseDto artWorkResponse, string search)
        {
            _cacheKeys.Add(search);
            _cacheService.Set(search, artWorkResponse);
        }

        public GetArtWorkResponseDto? GetMetropolitanMuseumCache(string search)
        {
            return _cacheService.Get<GetArtWorkResponseDto>(search);
        }
    }
}
