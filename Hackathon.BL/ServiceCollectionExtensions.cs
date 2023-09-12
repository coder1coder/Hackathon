using Amazon.S3;
using Hackathon.BL.Chat;
using Hackathon.BL.Email;
using Hackathon.BL.Event;
using Hackathon.BL.EventLog;
using Hackathon.BL.FileStorage;
using Hackathon.BL.Friend;
using Hackathon.BL.Notification;
using Hackathon.BL.Project;
using Hackathon.BL.Team;
using Hackathon.BL.User;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Abstraction.FileStorage;
using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
using Hackathon.BL.ApprovalApplications;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.Email;
using Hackathon.Common.Abstraction.Events;
using Microsoft.Extensions.Configuration;

namespace Hackathon.BL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>() ?? new EmailSettings();

        var s3Options = configuration.GetSection(nameof(S3Options)).Get<S3Options>() ?? new S3Options();

        services
            .AddScoped<IMessageBusService, MessageBusService>()
            .AddScoped<ITeamChatService, TeamBaseChatService>()
            .AddScoped<IEventChatService, EventBaseChatService>()
            .AddScoped<IApprovalApplicationService, ApprovalApplicationService>()
            .AddScoped<IEmailConfirmationService, EmailConfirmationService>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<ITemplateService, TemplateService>()
            .AddScoped<IEventService, EventService>()
            .AddScoped<IEventLogHandler, EventLogHandler>()
            .AddScoped<IEventLogService, EventLogService>()
            .AddScoped<IFileStorageService, FileStorageService>()
            .AddScoped<IFriendshipService, FriendshipService>()
            .AddScoped<INotificationService, NotificationService>()
            .AddScoped<IProjectService, ProjectService>()
            .AddScoped<ITeamService, TeamService>()
            .AddScoped<IPrivateTeamService, PrivateTeamService>()
            .AddScoped<IPublicTeamService, PublicTeamService>()
            .AddScoped<IUserProfileReactionService, UserProfileReactionService>()
            .AddScoped<IUserService, UserService>()
            .AddSingleton(new SmtpClient
            {
                Host = emailSettings.EmailSender?.Server,
                Port = emailSettings.EmailSender?.Port ?? default,
                EnableSsl = emailSettings.EmailSender?.EnableSsl ?? default,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(
                    emailSettings.EmailSender?.Username,
                    emailSettings.EmailSender?.Password)
            })
            .AddSingleton(new AmazonS3Client(
                s3Options.AccessKey,
                s3Options.SecretKey,
                new AmazonS3Config
                {
                    UseHttp = s3Options.UseHttp,
                    ServiceURL = s3Options.ServiceUrl,
                    ForcePathStyle = s3Options.ForcePathStyle
                })
            );

        return services;
    }
}
