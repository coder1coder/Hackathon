using FluentValidation;
using Hackathon.BL.Validation.Chat;
using Hackathon.BL.Validation.Common;
using Hackathon.BL.Validation.Event;
using Hackathon.BL.Validation.Project;
using Hackathon.BL.Validation.Team;
using Hackathon.BL.Validation.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.BL.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterValidators(this IServiceCollection services) => services
        .AddScoped<IValidator<ProjectCreateParameters>, ProjectCreateModelValidator>()
        .AddScoped<IValidator<CreateTeamModel>, CreateTeamModelValidator>()
        .AddScoped<IValidator<TeamMemberModel>, TeamAddMemberModelValidator>()
        .AddScoped<IValidator<GetListParameters<TeamFilter>>, GetListParametersValidator<TeamFilter>>()
        .AddScoped<IValidator<SignUpModel>, SignUpModelValidator>()
        .AddScoped<IValidator<SignInModel>, SignInModelValidator>()
        .AddScoped<IValidator<GetListParameters<UserFilter>>, GetListParametersValidator<UserFilter>>()
        .AddScoped<IValidator<GetListParameters<EventFilter>>, GetListParametersValidator<EventFilter>>()
        .AddScoped<IValidator<EventCreateParameters>, CreateEventModelValidator>()
        .AddScoped<IValidator<EventUpdateParameters>, UpdateEventModelValidator>()
        .AddScoped<IValidator<BaseEventParameters>, BaseEventParametersValidator>()
        .AddScoped<IValidator<ProjectUpdateFromGitParameters>, ProjectUpdateFromGitParametersValidator>()
        .AddScoped<IValidator<INewChatMessage>, CreateChatMessageValidator>()
        .AddScoped<IValidator<UpdateUserParameters>, UpdateUserModelValidator>();
}
