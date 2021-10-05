using KTalks.EmployeeOnboarding.Functions.Models.Responses;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Orchestrators
{
    public class Office365OrchestratorFunction
    {
        [FunctionName(nameof(AssignO365AndAzureLicence))]
        public Task<O365Licence> AssignO365AndAzureLicence(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var userId = context.GetInput<string>();

            try
            {
                // Example: Bubble exception from orchestrator.
                if (userId.StartsWith("9"))
                    throw new InvalidOperationException("User not supported for O365 licence.");

                // TODO: O365 Activities Here
                return Task.FromResult(new O365Licence
                {
                    LicenceAssigned = new List<string>
                    {
                        $"{userId}-Licence-1",
                        $"{userId}-Licence-2",
                    },
                    OutlookEnabled = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
