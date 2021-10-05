using KTalks.EmployeeOnboarding.Functions.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Activities
{
    public class LockServiceActivityFunctions
    {
        private readonly ILockService _lockService;

        public LockServiceActivityFunctions(ILockService lockService)
        {
            _lockService = lockService ?? throw new ArgumentNullException(nameof(lockService));
        }

        [FunctionName(nameof(AcquireLock))]
        public Task AcquireLock([ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            var resourceId = context.GetInput<string>();
            logger.LogInformation("Acquire orchestrator lock for {ResourceId}.", resourceId);
            return _lockService.AcquireLock(resourceId);
        }

        [FunctionName(nameof(RenewLock))]
        public Task RenewLock([ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            var (resourceId, lockId) = context.GetInput<(string, string)>();
            logger.LogInformation("Renew orchestrator lock for {ResourceId}.", resourceId);
            return _lockService.RenewLock(resourceId, lockId);
        }

        [FunctionName(nameof(ReleaseLock))]
        public Task ReleaseLock([ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            var (resourceId, lockId) = context.GetInput<(string, string)>();
            logger.LogInformation("Release orchestrator lock for {ResourceId}.", resourceId);
            return _lockService.ReleaseLock(resourceId, lockId);
        }
    }
}
