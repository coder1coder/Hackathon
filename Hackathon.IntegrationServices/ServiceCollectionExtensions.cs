using Hackathon.IntegrationServices.Github.Abstraction;
using Hackathon.IntegrationServices.Github.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Hackathon.IntegrationServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIntegrationServices(this IServiceCollection services) => services
        .AddScoped<IGitHubIntegrationService, GitHubIntegrationService>()
        .AddScoped<IGitIntegrationService, GitHubIntegrationService>()
        .AddSingleton(new HttpClient());
}
