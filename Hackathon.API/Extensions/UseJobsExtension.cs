using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hackathon.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Hackathon.API.Extensions
{
    public static class UseJobsExtension
    {
        public static IApplicationBuilder UseJobs(this IApplicationBuilder app)
        {
            var container = app.ApplicationServices.GetAutofacRoot();

            app.UseHangfireDashboard("/jobs", new DashboardOptions());

            RegisterJob<IChangeEventStatusJob>(container);

            return app;
        }

        private static void RegisterJob<T>(IComponentContext container) where T: IJob
        {
            var job = container.Resolve<T>();
            RecurringJob.RemoveIfExists(job.Name);
            RecurringJob.AddOrUpdate(job.Name, () => job.Execute(), job.CronInterval);
        }
    }
}