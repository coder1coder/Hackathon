using Autofac;
using Hackathon.Notification;
using Hackathon.Notification.IntegrationEvent;

namespace Hackathon.API;

public class Module: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IntegrationEventHub<NotificationPublishedIntegrationEvent>>()
            .As<IMessageHub<NotificationPublishedIntegrationEvent>>()
            .InstancePerLifetimeScope();
    }
}