using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KTalks.EmployeeOnboarding.Functions.Orchestrators
{
    public static class HelloDurableFunction
    {
        [FunctionName(nameof(HelloHttpStart))]
        public static async Task<HttpResponseMessage> HelloHttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            var instanceId = await starter.StartNewAsync(nameof(HelloOrchestratorFunction), null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName(nameof(HelloOrchestratorFunction))]
        public static async Task<List<string>> HelloOrchestratorFunction(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var responses = new List<string>();
            responses.Add(await context.CallActivityAsync<string>(nameof(SayHelloActivityFunction), "William"));
            responses.Add(await context.CallActivityAsync<string>(nameof(SayHelloActivityFunction), "Melissa"));
            responses.Add(await context.CallActivityAsync<string>(nameof(SayHelloActivityFunction), "Leah"));

            return responses;
        }

        [FunctionName(nameof(SayHelloActivityFunction))]
        public static string SayHelloActivityFunction([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }
    }
}