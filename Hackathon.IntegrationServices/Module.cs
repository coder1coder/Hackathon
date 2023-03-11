using System.Net.Http;
using Autofac;
using Hackathon.IntegrationServices.Github.Services;

namespace Hackathon.IntegrationServices;

public class Module: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GitHubIntegrationService>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterType<HttpClient>()
            .AsSelf();
    }
}
