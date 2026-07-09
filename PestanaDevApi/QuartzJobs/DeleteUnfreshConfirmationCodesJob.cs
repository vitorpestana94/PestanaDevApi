using PestanaDevApi.Interfaces.Services;
using Quartz;

namespace PestanaDevApi.QuartzJobs
{
    public class DeleteUnfreshConfirmationCodesJob: IJob
    {
        private readonly IConfirmationCodeService _service;

        public DeleteUnfreshConfirmationCodesJob(IConfirmationCodeService service) 
        {
            _service = service;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            await _service.DeleteUnfreshConfirmationCodes();
        }
    }
}
