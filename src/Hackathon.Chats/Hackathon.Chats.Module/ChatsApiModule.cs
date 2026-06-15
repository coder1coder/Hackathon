using System.Reflection;
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
using Hackathon.Chats.Infrastructure.Consumers.Teams;
using Hackathon.Chats.Infrastructure.IntegrationEvents;
using Hackathon.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Chats.Module;

public sealed class ChatsApiModule: ApiModule
{
    public override Assembly ConsumersAssembly => typeof(NewTeamMemberConsumer).Assembly;

    public override void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddScoped<ITeamChatService, TeamChatService>()
            .AddScoped<IEventChatService, EventChatService>();

        serviceCollection
            .AddScoped<IValidator<INewChatMessage>, NewChatMessageValidator>()
            .AddScoped<Hackathon.Common.Abstraction.IValidator<NewEventChatMessageModel>, NewEventChatMessageValidator>()
            .AddScoped<Hackathon.Common.Abstraction.IValidator<NewTeamChatMessage>, NewTeamChatMessageValidator>();

        serviceCollection
            .AddScoped<ITeamChatRepository, TeamChatRepository>()
            .AddScoped<IEventChatRepository, EventChatRepository>();

        serviceCollection.AddScoped<IEventChatHub, EventChatHub>();
        serviceCollection.AddScoped<ITeamChatHub, TeamChatHub>();

        serviceCollection.AddSingleton<IChatConnectionsProvider, ChatConnectionsProvider>();

        ConfigureDbContext<ChatsDbContext>(serviceCollection,
            connectionString: configuration.GetConnectionString("DefaultConnectionString"),
            enableSensitiveDataLogging: true);
    }

    public override void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder, AppSettings appSettings)
    {
        endpointRouteBuilder.MapHub<EventChatHub>(appSettings.Hubs.Chats.EventChats);
        endpointRouteBuilder.MapHub<TeamChatHub>(appSettings.Hubs.Chats.TeamChats);
    }
}
