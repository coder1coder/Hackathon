using System;
using Hackathon.DAL;
using Hackathon.Jobs;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.API.Extensions
{
    public static class JobsExtension
    {
        public static IServiceCollection AddJobs(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("JobsDatabaseConnectionString");
            return services.AddHangfire(configuration =>
            {
                configuration.UsePostgreSqlStorage(connectionString);
            });
        }

        public static IApplicationBuilder UseJobs(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var hangfireContext = serviceProvider.GetRequiredService<HangFireDbContext>();

            hangfireContext.Database.EnsureCreated();

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