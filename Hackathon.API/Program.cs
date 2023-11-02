using System.Collections.Generic;
using System.IO;
using Hackathon.API.Module;
using Hackathon.Chats.Module;
using Hackathon.Informing.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

namespace Hackathon.API;

public class Program
{
    public static readonly List<IApiModule> Modules = new();

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        MigrationsTool.ApplyMigrations(host, Modules);
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        Modules.Add(new ChatsApiModule());
        Modules.Add(new InformingApiModule());

        return Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, x) =>
            {
                x.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, false)
                    .AddEnvironmentVariables();
            })
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
                    .UseStartup(x =>
                        new Startup(x.Configuration, x.HostingEnvironment, Modules));
            });
    }
}
