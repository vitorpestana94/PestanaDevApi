using Microsoft.Extensions.Caching.Memory;
using PestanaDevApi.Services;

namespace PestanaDevApi.Tests.Services
{
    public class CacheServiceTests
    {
        private readonly CacheService _service = new(new MemoryCache(new MemoryCacheOptions()));

        [Fact]
        public void SetGetRemove_WorksByKey()
        {
            _service.Set("key", "value");

            Assert.Equal("value", _service.Get<string>("key"));

            _service.Remove("key");

            Assert.Null(_service.Get<string>("key"));
        }
    }

    public class SemaphoreServiceTests
    {
        [Fact]
        public async Task Work_RunsOneCallbackAtATime()
        {
            SemaphoreService service = new();
            int running = 0;
            int maxRunning = 0;

            async Task<int> CallBack()
            {
                maxRunning = Math.Max(maxRunning, Interlocked.Increment(ref running));
                await Task.Delay(20);
                Interlocked.Decrement(ref running);
                return 0;
            }

            await Task.WhenAll(Enumerable.Range(0, 5).Select(_ => service.Work(CallBack)));

            Assert.Equal(1, maxRunning);
        }
    }

    public class BoardHubConnectionServiceTests
    {
        [Fact]
        public void TracksConnectionsByUser()
        {
            BoardHubConnectionService service = new();
            Guid userId = Guid.NewGuid();

            service.AddConnection(userId, "a");
            service.AddConnection(userId, "b");
            service.RemoveConnection(userId, "a");

            Assert.Equal(["b"], service.GetConnections(userId));
            Assert.Empty(service.GetConnections(Guid.NewGuid()));
        }
    }
}
