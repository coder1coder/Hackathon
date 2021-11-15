using Autofac;
using Hackathon.Common.Abstraction;
using Hackathon.DAL.Repositories;

namespace Hackathon.DAL
{
    public class DalModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeamRepository>().As<ITeamRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerLifetimeScope();
        }
    }
}