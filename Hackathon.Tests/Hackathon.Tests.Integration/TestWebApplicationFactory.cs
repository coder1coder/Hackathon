using System.Threading;
using Amazon.S3;
using Amazon.S3.Model;
using Hackathon.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Hackathon.Tests.Integration;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = builder.Build();
        MigrationsTool.ApplyMigrations(host, Program.Modules);
        host.Start();
        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(x =>
            x.AddUserSecrets<Program>(true));

        builder.UseEnvironment("Tests");

        var s = builder.GetSetting("S3Options");

        var s3ClientMock = new Mock<IAmazonS3>();
        builder.ConfigureTestServices(x =>
        {
            x.AddSingleton(_ => s3ClientMock.Object);
        });
        InitMockStorageFileUpload(s3ClientMock);

        base.ConfigureWebHost(builder);
    }

    private static void InitMockStorageFileUpload(Mock<IAmazonS3> s3ClientMock)
    {
        s3ClientMock.Setup(x => x.ListBucketsAsync(default))
            .ReturnsAsync(new ListBucketsResponse());

        s3ClientMock.Setup(x => x.PutBucketAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PutBucketResponse());

        s3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default))
            .ReturnsAsync(new PutObjectResponse());
    }
}
