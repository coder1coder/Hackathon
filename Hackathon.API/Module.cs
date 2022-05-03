using Autofac;
using Hackathon.Notification;
using Hackathon.Notification.IntegrationEvent;

namespace Hackathon.API;

public class Module: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IntegrationEventHub<NotificationChangedIntegrationEvent>>()
            .As<IMessageHub<NotificationChangedIntegrationEvent>>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<IntegrationEventHub<ChatMessageChangedIntegrationEvent>>()
            .As<IMessageHub<ChatMessageChangedIntegrationEvent>>()
            .InstancePerLifetimeScope();
    }
}