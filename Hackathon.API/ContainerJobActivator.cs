using System;
using Hangfire;

namespace Hackathon.API;

public class ContainerJobActivator : JobActivator
{
    private readonly IServiceProvider _container;

    public ContainerJobActivator(IServiceProvider serviceProvider)
    {
        _container = serviceProvider;
    }

    public override object ActivateJob(Type type)
    {
        return _container.GetService(type);
    }
}