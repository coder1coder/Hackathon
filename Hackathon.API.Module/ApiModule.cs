using System;
using System.Collections.Generic;
using Hackathon.Common.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.API.Module;

public abstract class ApiModule: IApiModule
{
    public IList<Func<IServiceProvider, DbContext>> RegisteredDbContextFactories { get; } = new List<Func<IServiceProvider, DbContext>>();

    public abstract void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration);

    public abstract void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder, AppSettings appSettings);

    protected void ConfigureDbContext<TDbContext>(IServiceCollection serviceCollection, string connectionString,
        bool enableSensitiveDataLogging = false, int poolSize = 300) where TDbContext: DbContext
    {
        RegisteredDbContextFactories.Add(x=>x.GetRequiredService<TDbContext>());

        serviceCollection.AddDbContextPool<TDbContext>(options =>
        {
            options.UseNpgsql(connectionString, builder =>
            {
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                builder.EnableRetryOnFailure();
            });

            if (enableSensitiveDataLogging)
                options.EnableSensitiveDataLogging();
        }, poolSize);
    }
}
