using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Abstraction.FileStorage;
using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Abstraction.Notifications;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.DAL.Repositories;
using Hackathon.DAL.Repositories.Chat;
using Hackathon.DAL.Repositories.Events;
using Hackathon.DAL.Repositories.Team;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.DAL;

public static class ServiceCollectionExtensions
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<ITeamJoinRequestsRepository, TeamJoinRequestRepository>()
            .AddScoped<ITeamRepository, TeamRepository>()
            .AddScoped<ITeamChatRepository, TeamChatRepository>()
            .AddScoped<IEventChatRepository, EventChatRepository>()
            .AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>()
            .AddScoped<IEventLogRepository, EventLogRepository>()
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<IFileStorageRepository, FileStorageRepository>()
            .AddScoped<IFriendshipRepository, FriendshipRepository>()
            .AddScoped<INotificationRepository, NotificationRepository>()
            .AddScoped<IProjectRepository, ProjectRepository>()
            .AddScoped<IUserProfileReactionRepository, UserProfileReactionRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IEventAgreementRepository, EventAgreementRepository>()
            ;
    }
}
