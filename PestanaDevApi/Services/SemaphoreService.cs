
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Services
{
    public class SemaphoreService: ISemaphoreService
    {
        private readonly SemaphoreSlim _semaphore;

        public SemaphoreService(int maxConcurrency = 1)
        {
            _semaphore = new SemaphoreSlim(maxConcurrency);
        }

        public async Task<T> Work<T>(Func<Task<T>> callBack)
        {
            await _semaphore.WaitAsync();

            try
            {
               return await callBack();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
