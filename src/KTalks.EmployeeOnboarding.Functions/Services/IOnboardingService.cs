using System.Threading.Tasks;
using KTalks.EmployeeOnboarding.Functions.Models.Responses;

namespace KTalks.EmployeeOnboarding.Functions.Services
{
    public interface IOnboardingService
    {
        Task UpdateOnboardingWorkflowId(string requestId, string instanceId);

        Task MarkOnboardingRequestAsInProgress(string requestId);

        Task<AzureAdUser> CreateEmployeeInAzureAd(string employeeId);

        Task<EmployeeDetail> GetEmployeeDetailForOnboardingRequest(string requestId);

        Task<bool> IsEmployeePermanentStaff(string employeeId);

        Task UpdatePayrollWorkflowId(string requestId, string payrollOrchestratorInstanceId);
    }
}
