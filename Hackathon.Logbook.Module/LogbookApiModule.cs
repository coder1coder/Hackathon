using Hackathon.API.Module;
using Hackathon.Logbook.Abstraction.Handlers;
using Hackathon.Logbook.Abstraction.Repositories;
using Hackathon.Logbook.Abstraction.Services;
using Hackathon.Logbook.BL.Handlers;
using Hackathon.Logbook.BL.Services;
using Hackathon.Logbook.DAL;
using Hackathon.Logbook.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Logbook.Module;

public class LogbookApiModule: ApiModule
{
    public override void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddScoped<IEventLogHandler, EventLogHandler>()
            .AddScoped<IEventLogService, EventLogService>()
            .AddScoped<IEventLogRepository, EventLogRepository>();
        
        ConfigureDbContext<LogbookDbContext>(serviceCollection,
            configuration.GetConnectionString("DefaultConnectionString"),
            true);
    }
}
