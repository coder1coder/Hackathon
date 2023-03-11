using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Hackathon.API;

public class Program
{
    public static void Main(string[] args)
    {
        var appHost = CreateHostBuilder(args).Build();
        var logger = appHost.Services.GetRequiredService<ILogger>();

        try
        {
            appHost.Run();
        }
        catch (Exception e)
        {
            logger.Error(e, "Возникла ошибка во время запуска приложения: {Message}", e.Message);
            throw;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .UseSerilog((context, _, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>();
            });
}
