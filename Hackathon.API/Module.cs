using Autofac;
using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;

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

        builder.RegisterType<IntegrationEventHub<FriendshipChangedIntegrationEvent>>()
            .As<IMessageHub<FriendshipChangedIntegrationEvent>>()
            .InstancePerLifetimeScope();
    }
}
