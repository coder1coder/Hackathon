using Autofac;
using MapsterMapper;

namespace Hackathon.API
{
    public class ApiModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();
        }
    }
}