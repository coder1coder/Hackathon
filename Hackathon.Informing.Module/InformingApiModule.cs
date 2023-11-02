using System.Net;
using System.Net.Mail;
using Hackathon.API.Module;
using Hackathon.Common.Configuration;
using Hackathon.Informing.Abstractions.IntegrationEvents;
using Hackathon.Informing.Abstractions.Repositories;
using Hackathon.Informing.Abstractions.Services;
using Hackathon.Informing.BL.Services;
using Hackathon.Informing.DAL;
using Hackathon.Informing.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Informing.Module;

public class InformingApiModule: ApiModule
{
    public override void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>() ?? new EmailSettings();
        
        serviceCollection.AddSingleton(new SmtpClient
        {
            Host = emailSettings.EmailSender?.Server,
            Port = emailSettings.EmailSender?.Port ?? default,
            EnableSsl = emailSettings.EmailSender?.EnableSsl ?? default,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(
                emailSettings.EmailSender?.Username,
                emailSettings.EmailSender?.Password)
        });
            
        serviceCollection.AddScoped<INotificationService, NotificationService>();
        serviceCollection.AddScoped<INotificationRepository, NotificationRepository>();
        serviceCollection.AddScoped<IEmailService, EmailService>();
        serviceCollection.AddScoped<ITemplateService, TemplateService>();
        serviceCollection.AddScoped<ITemplateRepository, TemplateRepository>();

        serviceCollection.AddScoped<IInformingIntegrationEventsHub, InformingIntegrationEventsHub>();
        
        ConfigureDbContext<InformingDbContext>(serviceCollection,
            configuration.GetConnectionString("DefaultConnectionString"),
            true);
    }

    public override void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder, AppSettings appSettings)
    {
        endpointRouteBuilder.MapHub<InformingIntegrationEventsHub>(appSettings.Hubs.Notifications);
    }
}
