using System;
using Hackathon.API.Extensions;
using Hackathon.BL;
using Hackathon.Common.Configuration;
using Hackathon.DAL;
using Hackathon.DAL.Services;
using Hackathon.DAL.Mappings;
using System.Linq;
using Hackathon.Jobs;
using Hackathon.MessageQueue.Hubs;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Options;

namespace Hackathon.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthOptions>(Configuration.GetSection(nameof(AuthOptions)));
            services.Configure<AdministratorDefaults>(Configuration.GetSection(nameof(AdministratorDefaults)));

            var config = new TypeAdapterConfig();

            config.Apply(new EventEntityMapping(), new TeamEntityMapping(), new UserEntityMapping());

            services.AddSingleton(config);
            services.AddSingleton<IMapper, ServiceMapper>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"));
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine);
            });

            services
                .AddDalDependencies()
                .AddBlDependencies()
                .AddApiDependencies()
                .AddJobsDependencies();

            var origins = Configuration.GetSection(nameof(OriginsOptions)).Get<OriginsOptions>();

            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                    builder
                        .WithOrigins(origins.AllowUrls)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "OPTIONS"));
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(new MainActionFilter());
            });

            services.AddJobs(Configuration);
            services.AddSignalR();

            services.AddAuthentication(Configuration);
            services.AddAuthorization();
            services.AddSwagger();
        }

        public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IServiceProvider serviceProvider,
        ApplicationDbContext dbContext,
        ILogger<Startup> logger, IOptions<AdministratorDefaults> administratorDefaultsOptions)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hackathon.API v1");
            });
            }

            //not need for proxy server
            // app.UseHttpsRedirection();

            app.UseJobs(serviceProvider);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var messageHubPrefix = Configuration.GetValue<string>("MessageHubPrefix");

            app.UseCors("default");
            DbInitializer.Seed(dbContext, logger, administratorDefaultsOptions.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<EventMessageHub>(messageHubPrefix);
            });
        }
    }
}
