using System;
using System.Security.Claims;
using System.Text.Json;
using Hackathon.API.Consumers;
using Hackathon.API.Extensions;
using Hackathon.API.Mapping;
using Hackathon.BL;
using Hackathon.BL.Validation;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;
using Hackathon.IntegrationServices;
using Hackathon.Jobs;
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
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Hackathon.API;

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
        var authOptionsSection = Configuration.GetSection(nameof(AuthOptions));
        var authOptions = authOptionsSection?.Get<AuthOptions>();

        services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        services.Configure<DataSettings>(Configuration.GetSection(nameof(DataSettings)));
        services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
        services.Configure<RestrictedNames>(Configuration.GetSection(nameof(RestrictedNames)));
        services.Configure<AuthOptions>(authOptionsSection);

        var config = new TypeAdapterConfig();
        config.Scan(typeof(EventEntityMapping).Assembly, typeof(UserMapping).Assembly);
        services.AddSingleton(config);
        services.AddSingleton<IMapper, ServiceMapper>();

        var emailSettings = Configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>() ?? new EmailSettings();
        var s3Options = Configuration.GetSection(nameof(S3Options)).Get<S3Options>() ?? new S3Options();

        services
            .RegisterServices(emailSettings, s3Options)
            .RegisterValidators()
            .RegisterIntegrationEvents()
            .RegisterApiJobs()
            .RegisterIntegrationServices()
            .RegisterRepositories();

        var appConfig = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumers(typeof(EventLogConsumer).Assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);

                cfg.Host(new Uri(appConfig.MessageBrokerSettings.Host), host =>
                {
                    host.Username(appConfig.MessageBrokerSettings.UserName);
                    host.Password(appConfig.MessageBrokerSettings.Password);
                });
            });
        });

        var databaseServiceLifetime = ServiceLifetime.Scoped;

        if (_environment.EnvironmentName == "Tests")
        {
            databaseServiceLifetime = ServiceLifetime.Transient;
        }

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
            options.Filters.Add(new ExceptionActionFilter());
            options.Conventions.Add(new RouteTokenTransformerConvention(new LowerCaseRouteTransformer()));
        }).AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        services.AddSignalR(x=>
            x.EnableDetailedErrors = _environment.IsDevelopment());

        services.AddMemoryCache();
        services.AddAuthentication(authOptions);
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
        IOptions<AppSettings> appSettings,
        IOptions<DataSettings> dataSettings)
    {
        var appConfig = appSettings.Value;

        if (dataSettings?.Value?.ApplyMigrationsAtStart == true)
        {
            try
            {
                dbContext.Database.Migrate();
                DbInitializer.Seed(dbContext, logger, dataSettings.Value.AdministratorDefaults);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while applying migrations");
            }
        }

        if (!string.IsNullOrWhiteSpace(appConfig.PathBase))
            app.UsePathBase(appConfig.PathBase);

        if (_environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"{appConfig.PathBase?.Trim()}/swagger/v1/swagger.json", "Hackathon.API v1");
            c.DocExpansion(DocExpansion.None);
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
            endpoints.MapHub<IntegrationEventHub<FriendshipChangedIntegrationEvent>>(appConfig.Hubs.Friendship);
            endpoints.MapHub<IntegrationEventHub<EventStatusChangedIntegrationEvent>>(appConfig.Hubs.Events);
        });
    }
}
