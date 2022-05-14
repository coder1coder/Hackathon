using System;
using System.Security.Claims;
using Autofac;
using Hackathon.API.Consumers;
using Hackathon.API.Extensions;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;
using Mapster;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

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

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(typeof(AuditEventConsumer).Assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);

                    cfg.Host(new Uri(appConfig.RabbitMq.Host), host =>
                    {
                        host.Username(appConfig.RabbitMq.UserName);
                        host.Password(appConfig.RabbitMq.Password);
                    });
                });
            });

            var databaseServiceLifetime = ServiceLifetime.Scoped;

            if (_environment.EnvironmentName == "Tests")
                databaseServiceLifetime = ServiceLifetime.Transient;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"));

                if (appConfig.EnableSensitiveDataLogging == true)
                    options.EnableSensitiveDataLogging();
            }, databaseServiceLifetime);
            
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));

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
                options.Conventions.Add(new RouteTokenTransformerConvention(new LowerCaseRouteTransformer()));
            });

            services.AddSignalR(x=>x.EnableDetailedErrors = true);
            services.AddMemoryCache();
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

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var appConfig = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            
            builder.RegisterModule(new Module());
            builder.RegisterModule(new BL.Module(appConfig));
            builder.RegisterModule(new DAL.Module());
            builder.RegisterModule(new Jobs.Module());
        }

        public void Configure(
            IApplicationBuilder app,
            IServiceProvider serviceProvider,
            ApplicationDbContext dbContext,
            ILogger<Startup> logger,
            IOptions<AppSettings> appSettings)
        {
            var appConfig = appSettings.Value;

            try
            {
                dbContext.Database.Migrate();
                DbInitializer.Seed(dbContext, logger, appConfig.AdministratorDefaults);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<IntegrationEventHub<NotificationChangedIntegrationEvent>>(appConfig.Hubs.Notifications);
                endpoints.MapHub<IntegrationEventHub<ChatMessageChangedIntegrationEvent>>(appConfig.Hubs.Chat);
            });
        }
    }
}
