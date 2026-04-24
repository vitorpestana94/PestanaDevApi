using PestanaDevApi.Interfaces.Services;
using Quartz;

namespace PestanaDevApi.QuartzJobs
{
    public class DeleteExpiredRefreshTokensJob: IJob
    {
        public readonly ITokenService _service;

        public DeleteExpiredRefreshTokensJob(ITokenService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.DeleteExpiredRefreshTokens();
        }
    }
}
