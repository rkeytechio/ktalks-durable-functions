using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KTalks.EmployeeOnboarding.Functions.Orchestrators
{
    public class PayrollOrchestratorFunction
    {
        [FunctionName(nameof(PayrollOnboardingOrchestratorFunction))]
        public Task PayrollOnboardingOrchestratorFunction(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            try
            {
                // TODO: Payroll Activities Here
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
