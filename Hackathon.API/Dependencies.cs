using Hackathon.MessageQueue;
using Hackathon.MessageQueue.Hubs;
using Hackathon.MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.API
{
    public static class Dependencies
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services)
        {
            return services
                // .AddScoped<IMapper, Mapper>()
                .AddScoped<IMessageHub<EventMessage>, EventMessageHub>();
        }
    }
}