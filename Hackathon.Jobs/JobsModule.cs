using Autofac;

namespace Hackathon.Jobs
{
    public class JobsModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChangeEventStatusJob>().As<IChangeEventStatusJob>();
        }
    }
}