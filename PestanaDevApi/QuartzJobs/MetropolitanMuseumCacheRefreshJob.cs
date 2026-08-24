using PestanaDevApi.Interfaces.Services;
using Quartz;

namespace PestanaDevApi.QuartzJobs
{
    public class MetropolitanMuseumCacheRefreshJob: IJob
    {
        private readonly IMetropolitanMuseumIntegrationService _service;

        public MetropolitanMuseumCacheRefreshJob(IMetropolitanMuseumIntegrationService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.RefreshMetrpolitanMuseumCache();
        }
    }
}
