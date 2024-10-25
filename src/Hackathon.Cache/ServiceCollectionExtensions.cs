using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Hackathon.Cache;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, string connectionString)
    {
        services.AddOptions();

        var redisConfigurationOptions = ConfigurationOptions.Parse(connectionString);
        
        services.AddStackExchangeRedisCache(x =>
        {
            x.ConfigurationOptions = redisConfigurationOptions;
        });

        return services;
    }
}
