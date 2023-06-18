using System;
using System.IO;
using Hackathon.API.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

namespace Hackathon.API;

public class Program
{
    public static void Main(string[] args)
    {
        var appHost = CreateHostBuilder(args).Build();
        var logger = appHost.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            appHost.Migrate(logger);
            appHost.Run();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Возникла ошибка во время запуска приложения: {Message}", e.Message);
            throw;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .UseSerilog((context, _, configuration) => configuration
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder())
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .ReadFrom.Configuration(context.Configuration)
            )
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>();
            });
}
