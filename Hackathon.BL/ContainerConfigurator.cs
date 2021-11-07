using Autofac;
using Hackathon.BL.Event;

namespace Hackathon.BL
{
    public static class ContainerConfigurator
    {
        public static void ConfigureBLContainer(this ContainerBuilder builder)
        {
            builder.RegisterModule(new EventModule());
        }
    }
}
