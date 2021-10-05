using System;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Services
{
    public class LockService : ILockService
    {
        public Task<string> AcquireLock(string resourceId)
        {
            return AcquireLock(resourceId, new TimeSpan(0, 0, 5, 0));
        }

        public Task<string> AcquireLock(string resourceId, TimeSpan leaseDuration)
        {
            // TODO: Implement AcquireLock, If already acquired then exception.
            return Task.FromResult(new Guid().ToString());
        }

        public Task RenewLock(string resourceId, string lockId)
        {
            // TODO: Renew Lock, , If already expired then exception.
            return Task.CompletedTask;
        }

        public Task ReleaseLock(string resourceId, string lockId)
        {
            // TODO: Release Lock, if resourceId and lockId matches. Else exception.
            return Task.CompletedTask;
        }
    }
}
