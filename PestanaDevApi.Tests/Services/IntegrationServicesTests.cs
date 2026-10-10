using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Services;

namespace PestanaDevApi.Tests.Services
{
    public class NasaIntegrationServiceTests
    {
        private readonly Mock<INasaRequestService> _requestService = new();
        private readonly Mock<INasaCacheService> _cacheService = new();
        private readonly NasaIntegrationService _service;

        public NasaIntegrationServiceTests()
        {
            _service = new(_requestService.Object, _cacheService.Object);
        }

        [Fact]
        public async Task GetNasaAstronomyPictureOfDay_ReturnsBadRequest_WhenDateIsInTheFuture()
        {
            string tomorrow = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            NasaAstronomyPictureOfDayResponseDto response = await _service.GetNasaAstronomyPictureOfDay(new() { Date = tomorrow });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetNasaAstronomyPictureOfDay_ReturnsCache_WithoutRequestingNasa()
        {
            NasaAstronomyPictureOfDayResponseDto cached = new();
            _cacheService.Setup(c => c.GetNasaAstronomyPictureOfTodayCache()).Returns(cached);

            NasaAstronomyPictureOfDayResponseDto response = await _service.GetNasaAstronomyPictureOfDay(new());

            Assert.Same(cached, response);
            _requestService.Verify(r => r.GetNasaPictureOfToday(), Times.Never);
        }

        [Fact]
        public async Task GetNasaAstronomyPictureOfDay_ReturnsInternalServerError_WhenNasaFails()
        {
            NasaAstronomyPictureOfDayResponseDto response = await _service.GetNasaAstronomyPictureOfDay(new());

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task GetNasaAstronomyPictureOfPeriod_ReturnsBadRequest_WhenDatesAreInvalid()
        {
            NasaAstronomyPicturesOfPeriodResponseDto response = await _service.GetNasaAstronomyPictureOfPeriod(new() { StartDate = "2024-01-07", EndDate = "2024-01-01" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    public class NasaRequestServiceTests
    {
        private readonly Mock<IRequestService> _http = new();
        private readonly IConfiguration _config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["nasa.endpoint"] = "https://nasa" }).Build();

        [Fact]
        public async Task GetNasaPictureOfDay_ReturnsNull_WhenRequestFails()
        {
            _http.Setup(h => h.RequestAsync<NasaResponse>(It.IsAny<string>(), null, null, default, null)).ThrowsAsync(new HttpRequestException());

            Assert.Null(await new NasaRequestService(_config, _http.Object).GetNasaPictureOfDay("240101"));
        }

        [Fact]
        public void Constructor_Throws_WhenEndpointIsNotConfigured()
        {
            Assert.Throws<InvalidOperationException>(() => new NasaRequestService(new ConfigurationBuilder().Build(), _http.Object));
        }
    }

    public class MetropolitanMuseumIntegrationServiceTests
    {
        private readonly Mock<IMetropolitanMuseumRequestService> _requestService = new();
        private readonly Mock<IMetropolitanMuseumCacheService> _cacheService = new();
        private readonly MetropolitanMuseumIntegrationService _service;

        public MetropolitanMuseumIntegrationServiceTests()
        {
            _service = new(_requestService.Object, _cacheService.Object, new SemaphoreService());
        }

        [Fact]
        public async Task GetArtWork_ReturnsBadRequest_WhenSearchIsEmpty()
        {
            GetArtWorkResponseDto response = await _service.GetArtWork("");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetArtWork_ReturnsCache_WithoutRequestingTheMuseum()
        {
            GetArtWorkResponseDto cached = new();
            _cacheService.Setup(c => c.GetMetropolitanMuseumCache("van gogh")).Returns(cached);

            GetArtWorkResponseDto response = await _service.GetArtWork("  van gogh ");

            Assert.Same(cached, response);
            _requestService.Verify(r => r.SearchArtWorksIds(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetArtWork_ReturnsNotFound_WhenTheresNoArtWork()
        {
            _requestService.Setup(r => r.SearchArtWorksIds("unknown")).ReturnsAsync(new SearchArtWorksIdsResponseDto { Total = 0 });

            GetArtWorkResponseDto response = await _service.GetArtWork("unknown");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    public class CacheServicesTests
    {
        private readonly CacheService _cache = new(new MemoryCache(new MemoryCacheOptions()));

        [Fact]
        public void MetropolitanMuseumCache_DeletesAllCachedSearches()
        {
            MetropolitanMuseumCacheService service = new(_cache);
            service.SetMetropolitanMuseumCache(new GetArtWorkResponseDto(), "monet");
            service.SetMetropolitanMuseumCache(new GetArtWorkResponseDto(), "van gogh");

            service.DeleteMetropolitamMuseumCache();

            Assert.Null(service.GetMetropolitanMuseumCache("monet"));
            Assert.Null(service.GetMetropolitanMuseumCache("van gogh"));
        }

        [Fact]
        public void NasaCache_DeletesTodayAndPeriodCache()
        {
            NasaCacheService service = new(_cache);
            service.SetNasaAstronomyPictureOfTodayCache(new());
            service.SetNasaAstronomyPictureOfThePeriodCache(new());

            service.DeleteNasaCache();

            Assert.Null(service.GetNasaAstronomyPictureOfTodayCache());
            Assert.Null(service.GetNasaAstronomyPictureOfThePeriodCache());
        }

        [Fact]
        public void BoardCache_IsStoredByBoardAndDeletedOnlyForThatBoard()
        {
            BoardCacheService service = new(_cache);
            Guid boardOne = Guid.NewGuid();
            Guid boardTwo = Guid.NewGuid();
            service.SetBoardDataCache(boardOne, new(HttpStatusCode.OK, "one"));
            service.SetBoardDataCache(boardTwo, new(HttpStatusCode.OK, "two"));

            service.DeleteBoardDataCache(boardOne);

            Assert.Null(service.GetBoardDataCache(boardOne));
            Assert.Equal("two", service.GetBoardDataCache(boardTwo)!.Message);
        }
    }

    public class RequestServiceTests
    {
        private class FakeHandler(HttpStatusCode statusCode, string json) : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
                Task.FromResult(new HttpResponseMessage(statusCode) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
        }

        private record Sample(string Name);

        [Fact]
        public async Task RequestAsync_DeserializesTheResponseIgnoringCase()
        {
            RequestService service = new(new HttpClient(new FakeHandler(HttpStatusCode.OK, "{\"name\":\"value\"}")));

            Sample? response = await service.RequestAsync<Sample>("https://api/test");

            Assert.Equal("value", response!.Name);
        }

        [Fact]
        public async Task RequestAsync_Throws_WhenResponseIsNotSuccess()
        {
            RequestService service = new(new HttpClient(new FakeHandler(HttpStatusCode.InternalServerError, "")));

            await Assert.ThrowsAsync<HttpRequestException>(() => service.RequestAsync<Sample>("https://api/test"));
        }
    }
}
