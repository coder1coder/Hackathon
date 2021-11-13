using Autofac;
using Hackathon.Common.Configuration;
using MapsterMapper;
using Microsoft.Extensions.Options;

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