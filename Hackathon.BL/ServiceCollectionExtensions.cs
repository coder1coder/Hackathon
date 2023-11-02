using Amazon.S3;
using Hackathon.BL.ApprovalApplications;
using Hackathon.BL.Email;
using Hackathon.BL.Event;
using Hackathon.BL.EventLog;
using Hackathon.BL.FileStorage;
using Hackathon.BL.Friend;
using Hackathon.BL.Project;
using Hackathon.BL.Team;
using Hackathon.BL.User;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.FileStorage;
using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.BL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var s3Options = configuration.GetSection(nameof(S3Options)).Get<S3Options>() ?? new S3Options();

        services
            .AddScoped<IMessageBusService, MessageBusService>()
            .AddScoped<IApprovalApplicationService, ApprovalApplicationService>()
            .AddScoped<IEmailConfirmationService, EmailConfirmationService>()
            .AddScoped<IEventService, EventService>()
            .AddScoped<IEventLogHandler, EventLogHandler>()
            .AddScoped<IEventLogService, EventLogService>()
            .AddScoped<IFileStorageService, FileStorageService>()
            .AddScoped<IFriendshipService, FriendshipService>()
            .AddScoped<IProjectService, ProjectService>()
            .AddScoped<ITeamService, TeamService>()
            .AddScoped<IPrivateTeamService, PrivateTeamService>()
            .AddScoped<IPublicTeamService, PublicTeamService>()
            .AddScoped<IUserProfileReactionService, UserProfileReactionService>()
            .AddScoped<IUserService, UserService>()
            .AddSingleton<IAmazonS3, AmazonS3Client>(_=>new AmazonS3Client(
                s3Options.AccessKey,
                s3Options.SecretKey,
                new AmazonS3Config
                {
                    UseHttp = s3Options.UseHttp,
                    ServiceURL = s3Options.ServiceUrl,
                    ForcePathStyle = s3Options.ForcePathStyle
                })
            );

        return services;
    }
}
