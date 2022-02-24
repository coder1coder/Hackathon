using Autofac;
using Hackathon.Abstraction.Jobs;
using Hackathon.Jobs.BackgroundServices;
using Hackathon.Jobs.Services;
using Microsoft.Extensions.Hosting;

namespace Hackathon.Jobs;

public class Module: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EventStartNotifierBackgroundService>().As<IHostedService>().InstancePerDependency();
        builder.RegisterType<EventStartNotifierJob>().As<IEventStartNotifierJob>().InstancePerLifetimeScope();
    }
}