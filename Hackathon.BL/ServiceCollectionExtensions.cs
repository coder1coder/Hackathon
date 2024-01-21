using Hackathon.BL.ApprovalApplications;
using Hackathon.BL.Auth;
using Hackathon.BL.Email;
using Hackathon.BL.Events;
using Hackathon.BL.Friendship;
using Hackathon.BL.Projects;
using Hackathon.BL.Teams;
using Hackathon.BL.Users;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.Auth;
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
        => services
            .AddScoped<IMessageBusService, MessageBusService>()
            .AddScoped<IApprovalApplicationService, ApprovalApplicationService>()
            .AddScoped<IEmailConfirmationService, EmailConfirmationService>()
            .AddScoped<IEventService, EventService>()
            .AddScoped<IFriendshipService, FriendshipService>()
            .AddScoped<IProjectService, ProjectService>()
            .AddScoped<ITeamService, TeamService>()
            .AddScoped<IPrivateTeamService, PrivateTeamService>()
            .AddScoped<IPublicTeamService, PublicTeamService>()
            .AddScoped<IUserProfileReactionService, UserProfileReactionService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IPasswordHashService, PasswordHashService>()
            .AddScoped<IAuthService, AuthService>();
}
