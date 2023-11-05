using FluentValidation;
using Hackathon.API.Module;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Models.Events;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Chats.Abstractions.Repositories;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Chats.BL.Services;
using Hackathon.Chats.BL.Validators;
using Hackathon.Chats.DAL;
using Hackathon.Chats.DAL.Repositories;
using Hackathon.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Chats.Module;

public class ChatsApiModule: ApiModule
{
    public override void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddScoped<ITeamChatService, TeamChatService>()
            .AddScoped<IEventChatService, EventChatService>();

        serviceCollection
            .AddScoped<IValidator<INewChatMessage>, NewChatMessageValidator>()
            .AddScoped<Hackathon.Common.Abstraction.IValidator<NewEventChatMessage>, NewEventChatMessageValidator>()
            .AddScoped<Hackathon.Common.Abstraction.IValidator<NewTeamChatMessage>, NewTeamChatMessageValidator>();

        serviceCollection
            .AddScoped<ITeamChatRepository, TeamChatRepository>()
            .AddScoped<IEventChatRepository, EventChatRepository>();

        serviceCollection.AddSingleton<IChatsIntegrationEventsHub, ChatsIntegrationEventsHub>();

        ConfigureDbContext<ChatsDbContext>(serviceCollection,
            configuration.GetConnectionString("DefaultConnectionString"),
            true);
    }

    public override void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder, AppSettings appSettings)
    {
        endpointRouteBuilder.MapHub<ChatsIntegrationEventsHub>(appSettings.Hubs.Chat);
    }
}
