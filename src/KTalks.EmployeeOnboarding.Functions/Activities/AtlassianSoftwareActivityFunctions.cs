using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Activities
{
    public class AtlassianSoftwareActivityFunctions
    {
        [FunctionName(nameof(AssignJiraLicence))]
        public Task AssignJiraLicence([ActivityTrigger] string userId, ILogger log)
        {
            log.LogInformation(
                "Assigning Atlassian Jira licence to {UserId}.",
                userId);

            return Task.CompletedTask;
        }

        [FunctionName(nameof(AssignConfluenceLicence))]
        public Task AssignConfluenceLicence([ActivityTrigger] string userId, ILogger log)
        {
            log.LogInformation(
                "Assigning Atlassian Confluence licence to {UserId}.",
                userId);

            return Task.CompletedTask;
        }

        [FunctionName(nameof(AssignBitBucketLicence))]
        public Task AssignBitBucketLicence([ActivityTrigger] string userId, ILogger log)
        {
            log.LogInformation(
                "Assigning Atlassian BitBucket licence to {UserId}.",
                userId);

            return Task.CompletedTask;
        }
    }
}
