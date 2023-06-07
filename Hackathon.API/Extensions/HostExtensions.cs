using System;
using Hackathon.Common.Configuration;
using Hackathon.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.API.Extensions;

public static class HostExtensions
{
    public static void Migrate(this IHost host, ILogger logger)
    {
        using var scope = host.Services.CreateScope();

        var dataSettings = scope.ServiceProvider.GetRequiredService<IOptions<DataSettings>>();

        if (dataSettings?.Value?.ApplyMigrationsAtStart != true)
            return;

        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            DbInitializer.Seed(dbContext, logger, dataSettings.Value.AdministratorDefaults);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while applying migrations");
        }
    }
}
