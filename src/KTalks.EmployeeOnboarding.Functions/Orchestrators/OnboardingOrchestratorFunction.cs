using KTalks.EmployeeOnboarding.Functions.Activities;
using KTalks.EmployeeOnboarding.Functions.Models.Requests;
using KTalks.EmployeeOnboarding.Functions.Models.Responses;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Orchestrators
{
    public class OnboardingOrchestratorFunction
    {
        // TODO: Read from Config
        protected static readonly int VpnTokenCreateDurationSeconds = 5;
        protected static readonly int VpnTokenCreateCheckDurationSeconds = 1;

        protected static readonly int ResourceLockRetryIntervalSeconds = 1;
        protected static readonly int ResourceLockMaximumAttempts = 5;

        [FunctionName(nameof(OnboardEmployeeOrchestratorFunction))]
        public async Task OnboardEmployeeOrchestratorFunction(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            // Example: Get input object
            var onboardingRequest = context.GetInput<OnboardingRequest>();
            
            // Example: Replay Safe Logger.
            logger = context.CreateReplaySafeLogger(logger);

            var resourceId = $"{nameof(OnboardEmployeeOrchestratorFunction)}/{onboardingRequest.RequestId}";
            var lockId = string.Empty;

            try
            {
                var orchestratorLockRetry = new RetryOptions(
                    firstRetryInterval: TimeSpan.FromSeconds(ResourceLockRetryIntervalSeconds),
                    maxNumberOfAttempts: ResourceLockMaximumAttempts);

                lockId = await context.CallActivityWithRetryAsync<string>(
                    nameof(LockServiceActivityFunctions.AcquireLock),
                    orchestratorLockRetry,
                    resourceId);

                // Example: Call activity function from orchestrator with no return type
                await context.CallActivityAsync(
                    nameof(OnboardingActivityFunction.MarkOnboardingRequestAsInProgress),
                    onboardingRequest.RequestId);

                // Example: Call activity function from orchestrator with return type
                var employeeDetail = await context.CallActivityAsync<EmployeeDetail>(
                    nameof(OnboardingActivityFunction.GetEmployeeDetailForOnboardingRequest),
                    onboardingRequest.RequestId);

                // Example: Call activity (flaky) function with a retry.
                var azureAdUser = await context.CallActivityWithRetryAsync<AzureAdUser>(
                    nameof(OnboardingActivityFunction.CreateEmployeeInAzureAd),
                    new RetryOptions(TimeSpan.FromSeconds(5), 3),
                    employeeDetail.EmployeeId);

                // Example Control Flow (Conditional Flows)
                if (!employeeDetail.IsExternalContractor)
                {
                    // Example: Call (another) orchestrator function no wait/ parallelism
                    var payrollOrchestratorInstanceId = context.StartNewOrchestration(
                        nameof(PayrollOrchestratorFunction.PayrollOnboardingOrchestratorFunction),
                        employeeDetail.EmployeeId);

                    // PRO TIP: Save payroll orchestrator instance Id
                    await context.CallActivityAsync(
                        nameof(OnboardingActivityFunction.UpdatePayrollWorkflowInstanceId),
                        (onboardingRequest.RequestId, payrollOrchestratorInstanceId));

                    // Example: Set Custom status
                    context.SetCustomStatus(new
                    {
                        PayrollOrchestratorInstanceId = payrollOrchestratorInstanceId
                    });
                }

                // Example: Call (sub) orchestrator function with await.
                await context.CallSubOrchestratorAsync(
                    nameof(Office365OrchestratorFunction.AssignO365AndAzureLicence),
                    azureAdUser.UserId);

                // Example: Fan Out & Fan In
                // PRO TIP: Consider moving into Orchestrator Function
                var atlassianTasks = new List<Task>
                {
                    context.CallActivityAsync(
                        nameof(AtlassianSoftwareActivityFunctions.AssignJiraLicence),
                        azureAdUser.UserId),
                    context.CallActivityAsync(
                        nameof(AtlassianSoftwareActivityFunctions.AssignConfluenceLicence),
                        azureAdUser.UserId),
                    context.CallActivityAsync(
                        nameof(AtlassianSoftwareActivityFunctions.AssignBitBucketLicence),
                        azureAdUser.UserId)
                };

                await Task.WhenAll(atlassianTasks.ToArray());

                // Example: Long Running Operations.
                var vpnTokenRequestId = await context.CallActivityAsync<string>(
                    nameof(VpnActivityFunctions.CreateVpnTokenRequest),
                    azureAdUser.UserId);

                var timeoutDuration = context.CurrentUtcDateTime.AddSeconds(VpnTokenCreateDurationSeconds);
                var vpnTokenRequestResultCompleted = false;

                // Example: Create Timeouts for long running operations.
                while (timeoutDuration >= context.CurrentUtcDateTime)
                {
                    // Example: Renew lock for longer running operation.
                    await context.CallActivityAsync(
                        nameof(LockServiceActivityFunctions.RenewLock),
                        (resourceId, lockId));

                    // Example: Poll Check Status.
                    vpnTokenRequestResultCompleted = await context.CallActivityAsync<bool>(
                        nameof(VpnActivityFunctions.CheckVpnTokenRequest),
                        vpnTokenRequestId);

                    if (vpnTokenRequestResultCompleted)
                        break;

                    // Example: Use Create Timer to Pause
                    var nextCheck = context.CurrentUtcDateTime.AddSeconds(VpnTokenCreateCheckDurationSeconds);
                    await context.CreateTimer(nextCheck, CancellationToken.None);
                }

                // Example Timeout
                if (!vpnTokenRequestResultCompleted)
                    throw new TimeoutException("Time to create VPN access token expired. Aborting workflow.");

                // Example: Re-Call (Refresh) context after long runs.
                employeeDetail = await context.CallActivityAsync<EmployeeDetail>(
                    nameof(OnboardingActivityFunction.GetEmployeeDetailForOnboardingRequest),
                    onboardingRequest.RequestId);

                // Example: Call activity function new request input.
                await context.CallActivityAsync(
                    nameof(NotificationActivityFunction.SendWelcomeEmail),
                    new WelcomeNotificationRequest
                    {
                        Recipient = $"{employeeDetail.FirstName} ({employeeDetail.EmployeeId})",
                        Email = employeeDetail.Email
                    });
            }
            catch (Exception ex)
            {
                // Example: Handle Exception but bubble it up.
                logger.LogError(ex, ex.Message);
                throw;
            }
            finally
            {
                // Example: Finally release lock.
                await context.CallActivityAsync(
                    nameof(LockServiceActivityFunctions.ReleaseLock),
                    (resourceId, lockId));
            }
        }
    }
}
