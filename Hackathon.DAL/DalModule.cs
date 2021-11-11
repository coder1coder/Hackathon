using Autofac;
using Hackathon.Common;
using Hackathon.DAL.Repositories;

namespace Hackathon.DAL
{
    public class DalModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeamRepository>().As<ITeamRepository>().InstancePerLifetimeScope();
        }
    }
}