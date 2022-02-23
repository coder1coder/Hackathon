using Hackathon.Abstraction;
using Hackathon.Abstraction.FileStorage;
using Hackathon.DAL.Entities;
using Hackathon.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.DAL
{
    public static class Dependencies
    {
        public static IServiceCollection AddDalDependencies(this IServiceCollection services)
        {
            return services
                .AddScoped<IEventRepository, EventRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ITeamRepository, TeamRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<INotificationRepository, NotificationRepository>()
                .AddScoped<IFileStorageRepository, FileStorageRepository>()
                ;
        }
    }
}