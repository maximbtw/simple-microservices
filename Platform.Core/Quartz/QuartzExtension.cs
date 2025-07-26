using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Platform.Core.Quartz;

public static class QuartzExtension
{
    public static void AddQuartz(this IServiceCollection services)
    {
        services.AddQuartz(_ => { });
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
}