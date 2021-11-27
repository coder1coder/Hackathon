using Autofac;
using Hackathon.MessageQueue;
using Hackathon.MessageQueue.Hubs;
using Hackathon.MessageQueue.Messages;
using MapsterMapper;

namespace Hackathon.API
{
    public class ApiModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();

            builder.RegisterType<EventMessageHub>().As<IMessageHub<EventMessage>>().InstancePerLifetimeScope();
        }
    }
}