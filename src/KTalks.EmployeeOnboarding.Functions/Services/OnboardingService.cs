using KTalks.EmployeeOnboarding.Functions.Models.Responses;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Services
{
    public class OnboardingService : IOnboardingService
    {
        public Task UpdateOnboardingWorkflowId(string requestId, string instanceId)
        {
            return Task.CompletedTask;
        }

        public Task MarkOnboardingRequestAsInProgress(string requestId)
        {
            return Task.CompletedTask;
        }

        public Task<AzureAdUser> CreateEmployeeInAzureAd(string employeeId)
        {
            // Just a sample response. Making sure that employeeId = userId for Demo conditions.
            return Task.FromResult(new AzureAdUser
            {
                UserId = employeeId
            });
        }

        public Task<bool> IsEmployeePermanentStaff(string employeeId)
        {
            return Task.FromResult(true);
        }

        public Task<EmployeeDetail> GetEmployeeDetailForOnboardingRequest(string requestId)
        {
            // Just a sample response. Making sure that requestId = employeeId for Demo conditions.
            return Task.FromResult(new EmployeeDetail
            {
                EmployeeId = requestId,
                Email = $"{requestId}@email.com",
                FirstName = $"{requestId}-First Name",
                LastName = $"{requestId}-Last Name"
            });
        }

        public Task UpdatePayrollWorkflowId(string requestId, string payrollOrchestratorInstanceId)
        {
            return Task.CompletedTask;
        }
    }
}
