using PestanaDevApi.Interfaces.Services;
using Quartz;

namespace PestanaDevApi.QuartzJobs
{
    public class NasaCacheRefreshJob : IJob
    {
        public readonly INasaIntegrationService _service;

        public NasaCacheRefreshJob(INasaIntegrationService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.RefreshNasaCache();
        }
    }
}
