using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;
using CacheKeys = PestanaDevApi.Constants.CacheKeysConstants;

namespace PestanaDevApi.Services
{
    public class NasaCacheService: INasaCacheService
    {
        private readonly ICacheService _cacheService;
        private readonly string[] _keys;

        public NasaCacheService(ICacheService cacheService)
        {
            _cacheService = cacheService;
            _keys = [CacheKeys.GetNasaAstronomyPictureOfTodayKey, CacheKeys.NasaAstronomyPictureOfThePeriodKey];
        }

        public void DeleteNasaCache()
        {
            foreach(string key in _keys)
            {
                _cacheService.Remove(key);
            }
        }

        #region Today's Picture
        public NasaAstronomyPictureOfDayResponseDto? GetNasaAstronomyPictureOfTodayCache()
        {
            return _cacheService.Get<NasaAstronomyPictureOfDayResponseDto>(_keys[0]);
        }

        public void SetNasaAstronomyPictureOfTodayCache(NasaAstronomyPictureOfDayResponseDto nasaResponse)
        {
            _cacheService.Set(_keys[0], nasaResponse);
        }
        #endregion

        #region Period's Pictures
        public NasaAstronomyPicturesOfPeriodResponseDto? GetNasaAstronomyPictureOfThePeriodCache()
        {
            return _cacheService.Get<NasaAstronomyPicturesOfPeriodResponseDto?>(_keys[1]);
        }

        public void SetNasaAstronomyPictureOfThePeriodCache(NasaAstronomyPicturesOfPeriodResponseDto nasaResponse)
        {
            _cacheService.Set(_keys[1], nasaResponse);
        }
        #endregion
    }
}
