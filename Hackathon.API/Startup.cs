using System;
using Autofac;
using Hackathon.API.Extensions;
using Hackathon.BL;
using Hackathon.Common.Configuration;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Hackathon.Jobs;
using Hackathon.MessageQueue.Hubs;
using Hangfire;
using Hangfire.PostgreSql;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hackathon.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthOptions>(Configuration.GetSection(nameof(AuthOptions)));

            TypeAdapterConfig.GlobalSettings.Apply(new IRegister[]
            {
                new EventEntityMapping(),
                new TeamEntityMapping(),
                new UserEntityMapping()
            });

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"));
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine);
            }, 1000);

            services.AddControllers(options =>
            {
                options.Filters.Add(new MainActionFilter());
            });

            services
                .AddHangfire(configuration =>
                {
                    configuration
                        .UsePostgreSqlStorage(Configuration.GetConnectionString("JobsDatabaseConnectionString"));
                })
                .AddHangfireServer();

            services.AddSignalR();

            services.AddAuthentication(Configuration);

            services.AddSwagger();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureBLContainer();
            builder.RegisterModule(new ApiModule());
            builder.RegisterModule(new JobsModule());
            builder.RegisterModule(new DalModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hackathon.API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseJobs();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var messageHubPrefix = Configuration.GetValue<string>("MessageHubPrefix");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<EventMessageHub>(messageHubPrefix);
            });
        }
    }
}