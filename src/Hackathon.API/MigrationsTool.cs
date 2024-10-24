﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hackathon.API.Module;
using Hackathon.Common.Abstraction.User;
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

        if (dataSettings?.Value?.ApplyMigrationsAtStart != true)
        {
            return;
        }

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
                {
                    dbContexts.AddRange(modulesDbContexts);
                }
            }

            foreach (var dbContext in dbContexts)
            {
                logger.LogInformation("{Source} start applying migrations for {DbContext}",
                    nameof(MigrationsTool),
                    dbContext.GetType().Name);

                dbContext.Database.Migrate();
            }

            var passwordHashService = scope.ServiceProvider.GetRequiredService<IPasswordHashService>();
            DbInitializer.Seed(applicationDbContext, logger, dataSettings.Value.AdministratorDefaults, passwordHashService);
            
            logger.LogInformation("{Source} applying migrations finished", nameof(MigrationsTool));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{Source} Error while applying migrations. {ErrorMessage}",
                nameof(MigrationsTool),
                e.Message);
            throw;
        }
    }
}
