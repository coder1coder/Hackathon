using FluentValidation;
using Hackathon.BL.Auth;
using Hackathon.BL.Common.Validators;
using Hackathon.BL.Event;
using Hackathon.BL.Event.Validators;
using Hackathon.BL.Project;
using Hackathon.BL.Project.Validators;
using Hackathon.BL.Team;
using Hackathon.BL.Team.Validators;
using Hackathon.BL.User;
using Hackathon.BL.User.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.BL
{
    public static class Dependencies
    {
        public static IServiceCollection AddBlDependencies(this IServiceCollection services)
        {
            return services
                .AddScoped<IAuthService, AuthService>()

                .AddScoped<IValidator<ProjectCreateModel>, ProjectCreateModelValidator>()
                .AddScoped<IProjectService, ProjectService>()

                .AddScoped<IValidator<CreateTeamModel>, CreateTeamModelValidator>()
                .AddScoped<IValidator<TeamAddMemberModel>, TeamAddMemberModelValidator>()
                .AddScoped<IValidator<long>, TeamExistValidator>()
                .AddScoped<IValidator<GetFilterModel<TeamFilterModel>>, GetFilterModelValidator<TeamFilterModel>>()
                .AddScoped<ITeamService, TeamService>()

                .AddScoped<IValidator<SignUpModel>,SignUpModelValidator>()
                .AddScoped<IValidator<SignInModel>,SignInModelValidator>()
                .AddScoped<IUserService,UserService>()
                .AddScoped<IValidator<GetFilterModel<UserFilterModel>>,GetFilterModelValidator<UserFilterModel>>()

                .AddScoped<IValidator<CreateEventModel>, CreateEventModelValidator>()
                .AddScoped<IValidator<GetFilterModel<EventFilterModel>>,GetFilterModelValidator<EventFilterModel>>()
                .AddScoped<IEventService, EventService>();
        }
    }
}