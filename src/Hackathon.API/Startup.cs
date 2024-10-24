using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.API.Extensions;
using Hackathon.API.Module;
using Hackathon.BL;
using Hackathon.BL.Validation;
using Hackathon.Common.Models.Users;
using Hackathon.Configuration;
using Hackathon.Configuration.Auth;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Hackathon.Infrastructure;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.Hubs;
using Hackathon.IntegrationEvents.IntegrationEvents;
using Hackathon.IntegrationServices;
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
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Hackathon.API;

public class Startup
{
    private readonly IWebHostEnvironment _environment;
    private readonly IReadOnlyCollection<IApiModule> _modules;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment, IReadOnlyCollection<IApiModule> modules)
    {
        Configuration = configuration;
        _environment = environment;
        _modules = modules;
    }

    private IConfiguration Configuration { get; }

    private const string CorsPolicy = "CorsPolicy";

    public void ConfigureServices(IServiceCollection services)
    {
        var authSection = Configuration.GetSection("Auth");

        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        services.Configure<DataSettings>(Configuration.GetSection("DataSettings"));
        services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
        services.Configure<RestrictedNames>(Configuration.GetSection("RestrictedNames"));
        services.Configure<AuthenticateSettings>(authSection);
        services.Configure<RouteOptions>(x =>
        {
            x.LowercaseUrls = true;
        });

        var config = new TypeAdapterConfig();

        var solutionAssemblies = _modules.Select(x => x.GetType().Assembly).ToList();
        solutionAssemblies.AddRange(new []
        {
            typeof(EventMapping).Assembly
        });
        var solutionAssembliesArray = solutionAssemblies.ToArray();
        
        config.Scan(solutionAssembliesArray);

        services.AddSingleton(config);
        services.AddSingleton<IMapper, ServiceMapper>();

        services
            .RegisterInfrastructure()
            .RegisterServices()
            .RegisterValidators()
            .RegisterIntegrationEvents(_environment.IsDevelopment())
            .RegisterJobs(Configuration)
            .RegisterIntegrationServices()
            .RegisterRepositories();

        var appConfig = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()
                        ?? throw new AggregateException("Error while reading application configuration");

        var brokerConnectionString = Configuration.GetConnectionString("MessageBroker");

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            var consumerAssemblies = _modules
                ?.Select(module => module.ConsumersAssembly)
                .Where(assembly => assembly is not null)
                .ToArray();
            
            x.AddConsumers(consumerAssemblies);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
                cfg.Host(brokerConnectionString);
            });
        });

        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new AggregateException("Не удалось определить строку подключения для регистрации контекста базы данных");
            }

            options.UseNpgsql(connectionString, builder =>
            {
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                builder.EnableRetryOnFailure();
            });
            
            if (appConfig.EnableSensitiveDataLogging == true)
            {
                options.EnableSensitiveDataLogging();
            }
        });

        foreach (var apiModule in _modules)
        {
            apiModule.ConfigureServices(services, Configuration);
        }

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

        var authSettings = authSection.Get<AuthenticateSettings>();
        
        services
            .AddMemoryCache()
            .AddAuthentication(authSettings)
            .AddAuthorization(x =>
            {
                x.AddPolicy(nameof(UserRole.Administrator), p =>
                    p
                        .RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Role, ((int)UserRole.Administrator).ToString())
                );
            })
            .AddSwagger();

        services.AddHealthChecks();
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

        var appConfig = appSettings.Value;

        if (_environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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
        //UseHttpMetrics and UseMetricServer should be called after UseRouting
        .UseMetricServer()
        .UseHttpMetrics()
        .UseCors(CorsPolicy)
        .UseAuthentication()
        .UseAuthorization()
        .UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.MapControllers();

            endpointRouteBuilder.MapHub<IntegrationEventsHub<FriendshipChangedIntegrationEvent>>(appConfig.Hubs.Friendship);
            endpointRouteBuilder.MapHub<EventChangesIntegrationEventsHub>(appConfig.Hubs.Events);

            endpointRouteBuilder.MapHealthChecks("hc");
            endpointRouteBuilder.MapMetrics();

            foreach (var apiModule in _modules)
            {
                apiModule.ConfigureEndpoints(endpointRouteBuilder, appConfig);
            }
        });
    }

    private static async Task HandleError(HttpContext context)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        if (exception == null)
        {
            return;
        }

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
