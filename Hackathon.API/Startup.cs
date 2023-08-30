using System;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
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
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;
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

    private const string CorsPolicy = "CorsPolicy";

    public void ConfigureServices(IServiceCollection services)
    {
        var authOptionsSection = Configuration.GetSection(nameof(AuthOptions));
        var authOptions = authOptionsSection?.Get<AuthOptions>();

        services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        services.Configure<DataSettings>(Configuration.GetSection(nameof(DataSettings)));
        services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
        services.Configure<RestrictedNames>(Configuration.GetSection(nameof(RestrictedNames)));
        services.Configure<FileSettings>(Configuration.GetSection(nameof(FileSettings)));
        services.Configure<AuthOptions>(authOptionsSection);
        services.Configure<RouteOptions>(x =>
        {
            x.LowercaseUrls = true;
        });

        var config = new TypeAdapterConfig();
        config.Scan(typeof(EventMapping).Assembly, typeof(UserMapping).Assembly);
        services.AddSingleton(config);
        services.AddSingleton<IMapper, ServiceMapper>();

        services
            .RegisterServices(Configuration)
            .RegisterValidators()
            .RegisterIntegrationEvents()
            .RegisterApiJobs(Configuration)
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

        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"));
            if (appConfig.EnableSensitiveDataLogging == true)
                options.EnableSensitiveDataLogging();
        });

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));

        services
            .AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                    builder
                        .SetIsOriginAllowed(x => OriginHelper.IsAllowed(appConfig.OriginsOptions.AllowUrls, x))
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            }).AddControllers(options =>
            {
                options.Filters.Add(new ExceptionActionFilter());
            }).AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        services
            .AddMemoryCache()
            .AddAuthentication(authOptions)
            .AddAuthorization(x =>
            {
                x.AddPolicy(nameof(UserRole.Administrator), p =>
                    p
                        .RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Role, ((int)UserRole.Administrator).ToString())
                );
            })
            .AddSwagger()
            .AddSignalR(x=>
                x.EnableDetailedErrors = _environment.IsDevelopment());
    }

    public void Configure(
        IApplicationBuilder app,
        IServiceProvider serviceProvider,
        ApplicationDbContext dbContext,
        ILogger<Startup> logger,
        IOptions<AppSettings> appSettings)
    {
        //глобальный обработчик исключений должен быть первым
        app.UseExceptionHandler(p => p.Run(HandleError));

        app
            .UseMetricServer()
            .UseHttpMetrics();

        var appConfig = appSettings.Value;

        if (_environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.DocExpansion(DocExpansion.None);
        })
        .UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        })
        .UseRouting()
        .UseCors(CorsPolicy)
        .UseAuthentication()
        .UseAuthorization()
        .UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<IntegrationEventHub<NotificationChangedIntegrationEvent>>(appConfig.Hubs.Notifications);
            endpoints.MapHub<IntegrationEventHub<ChatMessageChangedIntegrationEvent>>(appConfig.Hubs.Chat);
            endpoints.MapHub<IntegrationEventHub<FriendshipChangedIntegrationEvent>>(appConfig.Hubs.Friendship);
            endpoints.MapHub<IntegrationEventHub<EventStatusChangedIntegrationEvent>>(appConfig.Hubs.Events);
        });
    }

    private static async Task HandleError(HttpContext context)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        if (exception == null)
            return;

        var response = MapException(exception);

        context.Response.StatusCode = response.Status!.Value;
        await context.Response.WriteAsJsonAsync(response);
    }

    private static ProblemDetails MapException(Exception exception)
    {
        if (exception is ValidationException)
        {
            return new ProblemDetails
            {
                Status = (int) HttpStatusCode.BadRequest,
                Type = exception.GetType().Name,
                Title = exception.Message,
                Detail = exception.InnerException?.Message
            };
        }

        return new ProblemDetails
        {
            Title = exception.Message,
            Detail = HttpStatusCode.InternalServerError.ToString(),
            Status = (int) HttpStatusCode.InternalServerError
        };
    }
}
