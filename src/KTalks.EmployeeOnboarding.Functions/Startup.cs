using KTalks.EmployeeOnboarding.Functions.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(KTalks.EmployeeOnboarding.Functions.Startup))]
namespace KTalks.EmployeeOnboarding.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ILockService, LockService>();
            builder.Services.AddTransient<IOnboardingService, OnboardingService>();
        }
    }
}