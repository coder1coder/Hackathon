using Amazon.S3;
using Autofac;
using Hackathon.Abstraction.EventLog;
using Hackathon.BL.EventLog;
using Hackathon.Common.Configuration;

namespace Hackathon.BL;

public class Module : Autofac.Module
{
    private readonly AppSettings _appSettings;

    public Module(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new Validation.Module());

        builder.RegisterType<EventLogHandler>().As<IEventLogHandler>().InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(Module).Assembly)
            .Where(x => !x.IsAbstract && x.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterInstance(new AmazonS3Client(
            _appSettings.S3Options.AccessKey,
            _appSettings.S3Options.SecretKey,
            new AmazonS3Config
            {
                UseHttp = _appSettings.S3Options.UseHttp,
                ServiceURL = _appSettings.S3Options.ServiceUrl,
                ForcePathStyle = _appSettings.S3Options.ForcePathStyle,
            })).AsSelf();
    }
}
