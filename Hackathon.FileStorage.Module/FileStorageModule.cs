using Amazon.S3;
using Hackathon.API.Module;
using Hackathon.FileStorage.Abstraction.Models;
using Hackathon.FileStorage.Abstraction.Repositories;
using Hackathon.FileStorage.Abstraction.Services;
using Hackathon.FileStorage.BL.Services;
using Hackathon.FileStorage.BL.Validators;
using Hackathon.FileStorage.Configuration;
using Hackathon.FileStorage.DAL;
using Hackathon.FileStorage.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.FileStorage.Module;

public class FileStorageModule: ApiModule
{
    public override void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<FileSettings>(configuration.GetSection(nameof(FileSettings)));
        
        var s3Options = configuration.GetSection(nameof(S3Options)).Get<S3Options>() ?? new S3Options();

        serviceCollection.AddScoped<IFileStorageService, FileStorageService>()
            .AddSingleton<IAmazonS3, AmazonS3Client>(_ => new AmazonS3Client(
                s3Options.AccessKey,
                s3Options.SecretKey,
                new AmazonS3Config
                {
                    UseHttp = s3Options.UseHttp,
                    ServiceURL = s3Options.ServiceUrl,
                    ForcePathStyle = s3Options.ForcePathStyle
                })
            );
        
        ConfigureDbContext<FileStorageDbContext>(serviceCollection,
            configuration.GetConnectionString("DefaultConnectionString"),
            true);

        serviceCollection.AddScoped<IFileStorageRepository, FileStorageRepository>();
        serviceCollection.AddScoped<FluentValidation.IValidator<IFileImage>, FileImageValidator>();
    }
}
