using Hackathon.BL.ApprovalApplications;
using Hackathon.BL.Email;
using Hackathon.BL.Event;
using Hackathon.BL.EventLog;
using Hackathon.BL.Friend;
using Hackathon.BL.Project;
using Hackathon.BL.Team;
using Hackathon.BL.User;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.BL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services
            .AddScoped<IMessageBusService, MessageBusService>()
            .AddScoped<IApprovalApplicationService, ApprovalApplicationService>()
            .AddScoped<IEmailConfirmationService, EmailConfirmationService>()
            .AddScoped<IEventService, EventService>()
            .AddScoped<IEventLogHandler, EventLogHandler>()
            .AddScoped<IEventLogService, EventLogService>()
            .AddScoped<IFriendshipService, FriendshipService>()
            .AddScoped<IProjectService, ProjectService>()
            .AddScoped<ITeamService, TeamService>()
            .AddScoped<IPrivateTeamService, PrivateTeamService>()
            .AddScoped<IPublicTeamService, PublicTeamService>()
            .AddScoped<IUserProfileReactionService, UserProfileReactionService>()
            .AddScoped<IUserService, UserService>();

        return services;
    }
}
