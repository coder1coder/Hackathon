using Autofac;
using Hackathon.BL.Event;
using Hackathon.BL.User;

namespace Hackathon.BL
{
    public static class ContainerConfigurator
    {
        public static void ConfigureBLContainer(this ContainerBuilder builder)
        {
            builder.RegisterModule(new EventModule());
            builder.RegisterModule(new UserModule());
        }
    }
}