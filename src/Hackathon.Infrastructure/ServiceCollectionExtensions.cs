using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Hackathon.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Зарегистрировать инфраструктурные зависимости.
    /// </summary>
    /// <param name="services">Коллекция служб.</param>
    /// <param name="configuration">Конфигурация.</param>
    /// <returns>Коллекция служб.</returns>
    public static IServiceCollection RegisterInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<IMessageBusService, MessageBusService>()
            .AddRedisDistributedCache(configuration.GetConnectionString("Redis"));

        return services;
    }

    /// <summary>
    /// Добавить распределенный кеш на базе Redis.
    /// </summary>
    /// <param name="services">Коллекция служб.</param>
    /// <param name="connectionString">Строка подключения к Redis.</param>
    /// <returns>Коллекция служб.</returns>
    private static void AddRedisDistributedCache(this IServiceCollection services,
        string connectionString)
    {
        services.AddOptions();

        var redisConfigurationOptions = ConfigurationOptions.Parse(connectionString);
        
        services.AddStackExchangeRedisCache(x =>
        {
            x.ConfigurationOptions = redisConfigurationOptions;
        });
    }
}
