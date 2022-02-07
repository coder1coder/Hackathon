using System;
using System.Security.Claims;
using Hackathon.API.Extensions;
using Hackathon.BL;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Hackathon.Notification;
using Hackathon.Notification.IntegrationEvent;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appConfig = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            var config = new TypeAdapterConfig();

            config.Scan(typeof(EventEntityMapping).Assembly, typeof(NotificationMapping).Assembly);

            services.AddSingleton(config);
            services.AddSingleton<IMapper, ServiceMapper>();

            // services.AddDbContext<HangFireDbContext>(options =>
            // {
            //     options.UseNpgsql(Configuration.GetConnectionString("JobsDatabaseConnectionString"));
            // });

            //Databases
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"));

                if (appConfig.EnableSensitiveDataLogging == true)
                    options.EnableSensitiveDataLogging();
            });

            services
                .AddDalDependencies()
                .AddBlDependencies()
                .AddApiDependencies()
                .AddNotificationDependencies();
                // .AddJobsDependencies();

            services.AddCors(options =>
            {
                options.AddPolicy("default", builder =>
                    builder
                        .WithOrigins(appConfig.OriginsOptions.AllowUrls)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(new MainActionFilter());
            });

            // services.AddJobs(Configuration);
            services.AddSignalR();

            services.AddAuthentication(appConfig);
            services.AddAuthorization(x =>
            {
                x.AddPolicy(nameof(UserRole.Administrator), p =>
                    p
                        .RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Role, ((int)UserRole.Administrator).ToString())
                    );
            });
            services.AddSwagger();
        }

        public void Configure(
            IApplicationBuilder app,
            IServiceProvider serviceProvider,
            ApplicationDbContext dbContext,
            ILogger<Startup> logger,
            IOptions<AppSettings> appSettings)
        {
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var appConfig = appSettings.Value;

            if (!string.IsNullOrWhiteSpace(appConfig.PathBase))
                app.UsePathBase(appConfig.PathBase);

            if (_environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{appConfig.PathBase?.Trim()}/swagger/v1/swagger.json", "Hackathon.API v1");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("default");
            // app.UseCors(x =>
            // {
            //     x
            //         .AllowAnyHeader()
            //         .AllowAnyMethod()
            //         .AllowAnyOrigin();
            // });

            //DbInitializer.Seed(dbContext, logger, administratorDefaultsOptions.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<IntegrationEventHub<NotificationPublishedIntegrationEvent>>(appConfig.NotificationsHubName);
            });
        }
    }
}
