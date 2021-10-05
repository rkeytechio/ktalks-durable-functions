using KTalks.EmployeeOnboarding.Functions.Models.Responses;
using KTalks.EmployeeOnboarding.Functions.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Activities
{
    public class OnboardingActivityFunction
    {
        private readonly IOnboardingService _onboardingService;

        public OnboardingActivityFunction(IOnboardingService onboardingService)
        {
            _onboardingService = onboardingService ?? throw new ArgumentNullException(nameof(onboardingService));
        }

        [FunctionName(nameof(MarkOnboardingRequestAsInProgress))]
        public async Task MarkOnboardingRequestAsInProgress(
            [ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            var requestId = context.GetInput<string>();

            logger.LogInformation(
                "Marking onboarding request as in progress {RequestId}.",
                requestId);

            await _onboardingService.MarkOnboardingRequestAsInProgress(requestId);
        }

        [FunctionName(nameof(GetEmployeeDetailForOnboardingRequest))]
        public async Task<EmployeeDetail> GetEmployeeDetailForOnboardingRequest(
            [ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            var requestId = context.GetInput<string>();

            logger.LogInformation(
                "Getting employee details for onboarding request {RequestId}.",
                requestId);

            return await _onboardingService.GetEmployeeDetailForOnboardingRequest(requestId);
        }

        [FunctionName(nameof(CreateEmployeeInAzureAd))]
        public async Task<AzureAdUser> CreateEmployeeInAzureAd(
            [ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            var employeeId = context.GetInput<string>();

            logger.LogInformation(
                "Create employee in Azure Ad {RequestId}.",
                employeeId);

            #region Bad Example

            var azureAdUser = await _onboardingService.CreateEmployeeInAzureAd(employeeId);
            logger.LogInformation(
                "Azure Ad user retrieved for {RequestId}. {AzureAdUser}",
                employeeId,
                azureAdUser);

            #endregion

            return await _onboardingService.CreateEmployeeInAzureAd(employeeId);
        }

        [FunctionName(nameof(UpdatePayrollWorkflowInstanceId))]
        public async Task UpdatePayrollWorkflowInstanceId(
            [ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            var (requestId, instanceId) = context.GetInput< (string, string)>();

            logger.LogInformation(
                "Getting employee details for onboarding request {RequestId}.",
                requestId);

            await _onboardingService.UpdatePayrollWorkflowId(requestId, instanceId);
        }
    }
}
