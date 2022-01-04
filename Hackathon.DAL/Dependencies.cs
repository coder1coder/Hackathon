using Hackathon.Common.Abstraction;
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
                .AddScoped<IProjectRepository, ProjectRepository>();
        }
    }
}