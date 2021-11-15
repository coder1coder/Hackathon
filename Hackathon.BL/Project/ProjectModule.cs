using Autofac;
using Hackathon.Common.Abstraction;

namespace Hackathon.BL.Project
{
    public class ProjectModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope();
        }
    }
}