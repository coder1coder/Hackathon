using FluentValidation;
using Hackathon.BL.Validation.ApprovalApplications;
using Hackathon.BL.Validation.Common;
using Hackathon.BL.Validation.Events;
using Hackathon.BL.Validation.Projects;
using Hackathon.BL.Validation.Teams;
using Hackathon.BL.Validation.Users;
using Hackathon.Common.Models;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.BL.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterValidators(this IServiceCollection services) => services
        .AddScoped<IValidator<IHasProjectIdentity>, ProjectIdentityParametersValidator>()
        .AddScoped<IValidator<BaseProjectParameters>, BaseProjectParametersValidator>()
        .AddScoped<Hackathon.Common.Abstraction.IValidator<ProjectCreationParameters>, ProjectCreateParametersValidator>()
        .AddScoped<Hackathon.Common.Abstraction.IValidator<ProjectUpdateParameters>, ProjectUpdateParametersValidator>()
        .AddScoped<IValidator<UpdateProjectFromGitBranchParameters>, UpdateProjectFromGitParametersValidator>()
        .AddScoped<IValidator<CreateTeamModel>, CreateTeamModelValidator>()
        .AddScoped<IValidator<TeamMemberModel>, TeamAddMemberModelValidator>()
        .AddScoped<IValidator<GetListParameters<TeamFilter>>, GetListParametersValidator<TeamFilter>>()
        .AddScoped<IValidator<CreateNewUserModel>, SignUpModelValidator>()
        .AddScoped<IValidator<SignInModel>, SignInModelValidator>()
        .AddScoped<IValidator<GetListParameters<UserFilter>>, GetListParametersValidator<UserFilter>>()
        .AddScoped<IValidator<GetListParameters<EventFilter>>, GetListParametersValidator<EventFilter>>()
        .AddScoped<IValidator<EventCreateParameters>, CreateEventModelValidator>()
        .AddScoped<IValidator<EventUpdateParameters>, UpdateEventModelValidator>()
        .AddScoped<IValidator<BaseEventParameters>, BaseEventParametersValidator>()
        .AddScoped<IValidator<UpdateUserParameters>, UpdateUserModelValidator>()
        .AddScoped<IValidator<ApprovalApplicationRejectParameters>, ApprovalApplicationRejectParametersValidator>()
        .AddScoped<IValidator<UpdatePasswordModel>, UpdatePasswordModelValidator>();
}
