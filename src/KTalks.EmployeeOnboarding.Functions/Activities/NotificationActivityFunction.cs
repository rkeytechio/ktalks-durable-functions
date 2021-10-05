using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Activities
{
    public class NotificationActivityFunction
    {
        [FunctionName(nameof(SendWelcomeEmail))]
        public Task SendWelcomeEmail([ActivityTrigger] string employeeId, ILogger log)
        {
            log.LogInformation(
                "Send welcome email to employee {RequestId}.",
                employeeId);

            return Task.CompletedTask;
        }
    }
}
