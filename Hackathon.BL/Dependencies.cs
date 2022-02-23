using Amazon.S3;
using FluentValidation;
using Hackathon.Abstraction;
using Hackathon.Abstraction.FileStorage;
using Hackathon.BL.Common.Validators;
using Hackathon.BL.Event;
using Hackathon.BL.Event.Validators;
using Hackathon.BL.FileStorage;
using Hackathon.BL.Notification;
using Hackathon.BL.Project;
using Hackathon.BL.Project.Validators;
using Hackathon.BL.Team;
using Hackathon.BL.Team.Validators;
using Hackathon.BL.User;
using Hackathon.BL.User.Validators;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.BL
{
    public static class Dependencies
    {
        public static IServiceCollection AddBlDependencies(this IServiceCollection services, AppSettings appSettings)
        {
            return services
                .AddScoped<IValidator<ProjectCreateModel>, ProjectCreateModelValidator>()
                .AddScoped<IProjectService, ProjectService>()

                .AddScoped<IValidator<CreateTeamModel>, CreateTeamModelValidator>()
                .AddScoped<IValidator<TeamAddMemberModel>, TeamAddMemberModelValidator>()
                .AddScoped<IValidator<long>, TeamExistValidator>()
                .AddScoped<IValidator<GetListModel<TeamFilterModel>>, GetFilterModelValidator<TeamFilterModel>>()
                .AddScoped<ITeamService, TeamService>()

                .AddScoped<IValidator<SignUpModel>,SignUpModelValidator>()
                .AddScoped<IValidator<SignInModel>,SignInModelValidator>()
                .AddScoped<IUserService,UserService>()
                .AddScoped<IValidator<GetListModel<UserFilterModel>>,GetFilterModelValidator<UserFilterModel>>()

                .AddScoped<IValidator<CreateEventModel>, CreateEventModelValidator>()
                .AddScoped<IValidator<GetListModel<EventFilterModel>>,GetFilterModelValidator<EventFilterModel>>()
                .AddScoped<IEventService, EventService>()

                .AddScoped<INotificationService, NotificationService>()

                .AddSingleton(new AmazonS3Client(
                    appSettings.S3Options.AccessKey,
                    appSettings.S3Options.SecretKey,
                    new AmazonS3Config()
                    {
                        UseHttp = appSettings.S3Options.UseHttp,
                        ServiceURL = appSettings.S3Options.ServiceUrl,
                        ForcePathStyle = appSettings.S3Options.ForcePathStyle
                    }))
                .AddScoped<IFileStorageService, FileStorageService>();
        }
    }
}