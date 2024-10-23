using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        => services
            .AddScoped<IMessageBusService, MessageBusService>();
}
