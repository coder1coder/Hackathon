using System;
using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Models.Events;
using Hackathon.Chats.Abstractions.Repositories;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.Notifications;
using Hackathon.Common.Abstraction.User;
using MapsterMapper;

namespace Hackathon.Chats.BL.Services;

public class EventChatService: BaseChatService<NewEventChatMessage, EventChatMessage>, IEventChatService
{
    private readonly IEventRepository _eventRepository;
    private readonly Common.Abstraction.IValidator<NewEventChatMessage> _validator;

    public EventChatService(
        IEventChatRepository eventChatRepository,
        IEventRepository eventRepository,
        IChatsIntegrationEventsHub integrationEventsHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        Common.Abstraction.IValidator<NewEventChatMessage> validator,
        IMapper mapper):base(eventChatRepository, integrationEventsHub, userRepository, notificationService, mapper)
    {
        _eventRepository = eventRepository;
        _validator = validator;
    }

    protected override Task<Result> ValidateAsync(NewEventChatMessage message)
        => _validator.ValidateAsync(message);

    public new Task<Result> SendAsync(long ownerId, NewEventChatMessage newEventChatMessage)
        => base.SendAsync(ownerId, newEventChatMessage);

    protected override Task PublicIntegrationEvent(Guid messageId, NewEventChatMessage newMessage)
        => IntegrationEventsHub.PublishAll(new EventChatNewMessageIntegrationEvent
        {
            EventId = newMessage.EventId,
            MessageId = messageId
        });

    protected override Task EnrichMessageBeforeSaving<TChatMessageModel>(INewChatMessage newChatMessage, TChatMessageModel chatMessage)
    {
        if (newChatMessage is NewEventChatMessage createEventChatMessage && chatMessage is EventChatMessage eventChatMessage)
        {
            eventChatMessage.EventId = createEventChatMessage.EventId;
        }

        return Task.CompletedTask;
    }

    protected override async Task<long[]> GetUserIdsToNotify(long ownerId, NewEventChatMessage newChatMessage)
    {
        var @event = await _eventRepository.GetAsync(newChatMessage.EventId);

        return @event.Teams?.SelectMany(x=>x.Members?.Select(m=>m.Id))
            .Where(x=> x != ownerId)
            .ToArray();
    }
}
