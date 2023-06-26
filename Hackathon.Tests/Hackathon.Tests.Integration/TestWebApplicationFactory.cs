using System;
using System.IO;
using BackendTools.Common.Models;
using Hackathon.API;
using Hackathon.API.Extensions;
using Hackathon.Common.Abstraction.FileStorage;
using Hackathon.Common.Models.FileStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hackathon.Tests.Integration;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = builder.Build();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        host.Migrate(logger);
        host.Start();
        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(x =>
            x.AddUserSecrets<Program>(true));

        builder.UseEnvironment("Tests");

        var mock = new Mock<IFileStorageService>();
        builder.ConfigureTestServices(x => x.AddScoped(_ => mock.Object));
        InitMockStorageFileUpload(mock);

        base.ConfigureWebHost(builder);
    }

    private static void InitMockStorageFileUpload(Mock<IFileStorageService> fileStorageMock)
    {
        fileStorageMock.Setup(x =>
            x.UploadAsync(
                It.IsAny<Stream>(),
                It.IsAny<Bucket>(),
                It.IsAny<string>(),
                It.IsAny<long?>(),
                It.IsAny<bool>()
            )).ReturnsAsync(new StorageFile{ Id = Guid.NewGuid(), BucketName = Bucket.Avatars.ToBucketName() }
        );

        fileStorageMock.Setup(x =>
                x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success);
    }
}
