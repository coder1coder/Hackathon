using System.Net;
using System.Net.Mail;
using Amazon.S3;
using Autofac;
using Hackathon.Abstraction.EventLog;
using Hackathon.BL.EventLog;
using Hackathon.Common.Configuration;

namespace Hackathon.BL;

public class Module : Autofac.Module
{
    private readonly EmailSettings _emailSettings;
    private readonly S3Options _s3Options;

    public Module(EmailSettings emailSettings, S3Options s3Options)
    {
        _emailSettings = emailSettings;
        _s3Options = s3Options;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EventLogHandler>().As<IEventLogHandler>().InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(Module).Assembly)
            .Where(x => !x.IsAbstract && x.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterInstance(new SmtpClient
            {
                Host = _emailSettings.EmailSender.Server,
                Port = _emailSettings.EmailSender.Port,
                EnableSsl = _emailSettings.EmailSender.EnableSsl,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_emailSettings.EmailSender.Username, _emailSettings.EmailSender.Password)
            })
            .AsSelf();

        builder.RegisterInstance(new AmazonS3Client(
            _s3Options.AccessKey,
            _s3Options.SecretKey,
            new AmazonS3Config
            {
                UseHttp = _s3Options.UseHttp,
                ServiceURL = _s3Options.ServiceUrl,
                ForcePathStyle = _s3Options.ForcePathStyle
            })).AsSelf();
    }
}
