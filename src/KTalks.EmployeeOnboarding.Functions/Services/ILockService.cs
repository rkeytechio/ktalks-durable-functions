using System;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Services
{
    public interface ILockService
    {
        Task<string> AcquireLock(string resourceId);

        Task<string> AcquireLock(string resourceId, TimeSpan leaseDuration);

        Task RenewLock(string resourceId, string lockId);

        Task ReleaseLock(string resourceId, string lockId);
    }
}
