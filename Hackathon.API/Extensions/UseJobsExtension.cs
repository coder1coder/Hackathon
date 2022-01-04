using System;
using Hackathon.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.API.Extensions
{
    public static class UseJobsExtension
    {
        public static IApplicationBuilder UseJobs(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseHangfireDashboard("/jobs", new DashboardOptions());
            RegisterJob<IChangeEventStatusJob>(serviceProvider);
            return app;
        }
        private static void RegisterJob<T>(IServiceProvider serviceProvider) where T: IJob
        {
            var job = (T)serviceProvider.GetRequiredService(typeof(T));
            RecurringJob.RemoveIfExists(job.Name);
            RecurringJob.AddOrUpdate(job.Name, () => job.Execute(), job.CronInterval);
        }
    }
}