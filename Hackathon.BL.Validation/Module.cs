using Autofac;
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

namespace Hackathon.BL.Validation
{
    public class Module: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectCreateModelValidator>().As<IValidator<ProjectCreateParameters>>().InstancePerLifetimeScope();
            builder.RegisterType<CreateTeamModelValidator>().As<IValidator<CreateTeamModel>>().InstancePerLifetimeScope();
            builder.RegisterType<TeamAddMemberModelValidator>().As<IValidator<TeamMemberModel>>().InstancePerLifetimeScope();
            builder.RegisterType<GetListParametersValidator<TeamFilter>>().As<IValidator<GetListParameters<TeamFilter>>>().InstancePerLifetimeScope();
            builder.RegisterType<SignUpModelValidator>().As<IValidator<SignUpModel>>().InstancePerLifetimeScope();
            builder.RegisterType<SignInModelValidator>().As<IValidator<SignInModel>>().InstancePerLifetimeScope();
            builder.RegisterType<GetListParametersValidator<UserFilter>>().As<IValidator<GetListParameters<UserFilter>>>().InstancePerLifetimeScope();
            builder.RegisterType<GetListParametersValidator<EventFilter>>().As<IValidator<GetListParameters<EventFilter>>>().InstancePerLifetimeScope();
            builder.RegisterType<CreateEventModelValidator>().As<IValidator<EventCreateParameters>>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateEventModelValidator>().As<IValidator<EventUpdateParameters>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseEventParametersValidator>().As<IValidator<BaseEventParameters>>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectUpdateFromGitParametersValidator>().As<IValidator<ProjectUpdateFromGitParameters>>().InstancePerLifetimeScope();
            builder.RegisterType<CreateChatMessageValidator>().As<IValidator<ICreateChatMessage>>().InstancePerLifetimeScope();
        }
    }
}
