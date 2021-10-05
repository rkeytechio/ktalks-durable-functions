using KTalks.EmployeeOnboarding.Functions.Models.Requests;
using KTalks.EmployeeOnboarding.Functions.Orchestrators;
using KTalks.EmployeeOnboarding.Functions.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Starters
{
    public class OnboardingStarterFunction
    {
        private readonly IOnboardingService _onboardingService;

        public OnboardingStarterFunction(IOnboardingService onboardingService)
        {
            _onboardingService = onboardingService ?? throw new ArgumentNullException(nameof(onboardingService));
        }

        [FunctionName(nameof(BeginEmployeeOnboardingQueueTriggerFunction))]
        public async Task BeginEmployeeOnboardingQueueTriggerFunction(
            [QueueTrigger("%BeginEmployeeOnboardingQueueName%", Connection = "KTalksStorage")] string queueMessage,
            [DurableClient] IDurableOrchestrationClient durableClient,
            ILogger logger)
        {
            // Get queue message or deserialized request object.
            var onboardingRequest = JsonConvert.DeserializeObject<OnboardingRequest>(queueMessage);

            logger.LogInformation(
                "Starting Employee Onboarding Orchestrator for Employee Id: {RequestId}.",
                onboardingRequest.RequestId);

            // Start Main (Entry) Orchestrator
            var instanceId = await durableClient.StartNewAsync(
                nameof(OnboardingOrchestratorFunction.OnboardEmployeeOrchestratorFunction),
                onboardingRequest);

            logger.LogInformation(
                "Started Employee Onboarding Orchestrator for Employee Id: {RequestId}, Instance Id: {InstanceId}",
                onboardingRequest.RequestId,
                instanceId);

            // Pro Tip: Save the instance Id against the original request.
            await _onboardingService.UpdateOnboardingWorkflowId(onboardingRequest.RequestId, instanceId);
        }
    }
}
