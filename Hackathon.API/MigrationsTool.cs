using System;
using System.Collections.Generic;
using System.Linq;
using Hackathon.API.Module;
using Hackathon.Configuration;
using Hackathon.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.API;

public class MigrationsTool
{
    public static void ApplyMigrations(IHost host, IReadOnlyCollection<IApiModule> apiModules = null)
    {
        using var scope = host.Services.CreateScope();

        var dataSettings = scope.ServiceProvider.GetRequiredService<IOptions<DataSettings>>();

        if (dataSettings?.Value?.ApplyMigrationsAtStart != true) return;

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationsTool>>();

        try
        {
            var dbContexts = new List<DbContext>();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContexts.Add(applicationDbContext);

            if (apiModules is { Count: > 0 })
            {
                var modulesDbContexts = apiModules.SelectMany(module => module.RegisteredDbContextFactories)
                    .Select(x => x?.Invoke(scope.ServiceProvider))
                    .ToArray();

                if (modulesDbContexts is {Length: > 0})
                    dbContexts.AddRange(modulesDbContexts);
            }

            foreach (var dbContext in dbContexts)
                dbContext.Database.Migrate();

            DbInitializer.Seed(applicationDbContext, logger, dataSettings.Value.AdministratorDefaults);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while applying migrations");
        }
    }
}
