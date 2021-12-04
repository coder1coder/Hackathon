using Autofac;
using FluentValidation;
using Hackathon.BL.Common.Validators;
using Hackathon.BL.Team.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team
{
    public class TeamModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreateTeamModelValidator>().As<IValidator<CreateTeamModel>>();
            builder.RegisterType<TeamAddMemberModelValidator>().As<IValidator<TeamAddMemberModel>>();
            builder.RegisterType<TeamExistValidator>().As<IValidator<long>>();
            builder.RegisterType<GetFilterModelValidator<TeamFilterModel>>().As<IValidator<GetFilterModel<TeamFilterModel>>>();
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
        }
    }
}