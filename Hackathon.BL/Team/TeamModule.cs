using Autofac;
using FluentValidation;
using Hackathon.BL.Team.Validators;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;

namespace Hackathon.BL.Team
{
    public class TeamModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreateTeamModelValidator>().As<IValidator<CreateTeamModel>>();
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
        }
    }
}