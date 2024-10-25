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
using Hackathon.Common.Abstraction.User;
using Hackathon.Informing.Abstractions.Services;
using MapsterMapper;

namespace Hackathon.Chats.BL.Services;

public class EventChatService: BaseChatService<NewEventChatMessageModel, EventChatMessage>, IEventChatService
{
    private readonly IEventRepository _eventRepository;
    private readonly Common.Abstraction.IValidator<NewEventChatMessageModel> _validator;
    private readonly IEventChatHub _eventChatHub;

    public EventChatService(
        IEventChatRepository eventChatRepository,
        IEventRepository eventRepository,
        IEventChatHub eventChatHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        Common.Abstraction.IValidator<NewEventChatMessageModel> validator,
        IMapper mapper):base(eventChatRepository, userRepository, notificationService, mapper)
    {
        _eventRepository = eventRepository;
        _eventChatHub = eventChatHub;
        _validator = validator;
    }

    protected override Task<Result> ValidateNewMessageAsync(NewEventChatMessageModel message)
        => _validator.ValidateAsync(message);

    public new Task<Result> SendMessageAsync(long ownerId, NewEventChatMessageModel newEventChatMessage)
    {
        return base.SendMessageAsync(ownerId, newEventChatMessage);
    }

    protected override Task PublicIntegrationEvent(Guid messageId, NewEventChatMessageModel newMessage)
    {
        return _eventChatHub.SendEventAsync(new EventChatNewMessageIntegrationEvent
        {
            EventId = newMessage.EventId,
            MessageId = messageId
        });
    }

    protected override Task EnrichMessageBeforeSaving<TChatMessageModel>(INewChatMessage newChatMessage, TChatMessageModel chatMessage)
    {
        if (newChatMessage is NewEventChatMessageModel createEventChatMessage && chatMessage is EventChatMessage eventChatMessage)
        {
            eventChatMessage.EventId = createEventChatMessage.EventId;
        }

        return Task.CompletedTask;
    }

    protected override async Task<long[]> GetUserIdsToNotify(long ownerId, NewEventChatMessageModel newChatMessage)
    {
        var @event = await _eventRepository.GetAsync(newChatMessage.EventId);

        return @event.Teams?.SelectMany(x=>x.Members?.Select(m=>m.Id))
            .Where(x=> x != ownerId)
            .ToArray();
    }
}
