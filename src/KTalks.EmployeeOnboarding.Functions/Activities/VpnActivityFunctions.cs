using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace KTalks.EmployeeOnboarding.Functions.Activities
{
    public class VpnActivityFunctions
    {
        [FunctionName(nameof(CreateVpnTokenRequest))]
        public string CreateVpnTokenRequest([ActivityTrigger] string userId, ILogger log)
        {
            log.LogInformation(
                "Creating a request to issue a VPN Token for {UserId}.",
                userId);

            // Generate a New Guid as a reference to the request.
            return $"{userId}-{Guid.NewGuid()}";
        }

        [FunctionName(nameof(CheckVpnTokenRequest))]
        public Task<bool> CheckVpnTokenRequest([ActivityTrigger] string requestId, ILogger log)
        {
            log.LogInformation(
                "Checking VPN Token request {VpnTokenRequestId}.",
                requestId);

            return Task.FromResult(!requestId.StartsWith("7"));
        }
    }
}
