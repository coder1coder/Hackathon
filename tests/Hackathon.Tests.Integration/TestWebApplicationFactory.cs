using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Xunit;

namespace Hackathon.Tests.Integration;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestWebApplicationFactory : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder()
        .WithName(ResolveContainerName("postgres"))
        .Build();

    private readonly RabbitMqContainer _rabbitmqContainer = new RabbitMqBuilder()
        .WithName(ResolveContainerName("rabbitmq"))
        .Build();

    private readonly MinioContainer _minioContainer = new MinioBuilder()
        .WithName(ResolveContainerName("minio"))
        .Build();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = builder.Build();
        MigrationsTool.ApplyMigrations(host, Program.Modules);
        host.Start();
        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        builder.UseEnvironment("Tests");
        
        builder.ConfigureAppConfiguration((x) =>
        {
            x.AddUserSecrets<Program>(true);
            x.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnectionString", _databaseContainer.GetConnectionString()},
                { "ConnectionStrings:MessageBroker", _rabbitmqContainer.GetConnectionString()},
                { "S3Options:ServiceUrl", _minioContainer.GetConnectionString() },
                { "S3Options:AccessKey", _minioContainer.GetAccessKey() },
                { "S3Options:SecretKey", _minioContainer.GetSecretKey() }
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
        await _rabbitmqContainer.StartAsync();
        await _minioContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _databaseContainer.DisposeAsync();
        await _rabbitmqContainer.DisposeAsync();
        await _minioContainer.DisposeAsync();
    }

    private static string ResolveContainerName(string serviceName)
        => $"hackathon_tests_{serviceName}_{Guid.NewGuid().ToString()[..8]}";
}
