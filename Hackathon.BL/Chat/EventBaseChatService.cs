using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Event;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MapsterMapper;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.BL.Chat;

public class EventBaseChatService: BaseChatService<NewEventChatMessage, EventChatMessage>, IEventChatService
{
    private readonly IEventRepository _eventRepository;

    public EventBaseChatService(
        IEventChatRepository eventChatRepository,
        IEventRepository eventRepository,
        IMessageHub<ChatMessageChangedIntegrationEvent> chatMessageHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        IValidator<INewChatMessage> newMessageValidator,
        IMapper mapper):base(eventChatRepository, chatMessageHub, userRepository, notificationService, newMessageValidator, mapper)
    {
        _eventRepository = eventRepository;
    }

    public new Task<Result> SendAsync(long ownerId, NewEventChatMessage newEventChatMessage)
        => base.SendAsync(ownerId, newEventChatMessage);

    public Task<Result<BaseCollection<EventChatMessage>>> GetListAsync(long eventId, int offset = 0, int limit = 300)
        => base.GetListAsync(eventId.ToString(), offset, limit);

    protected override void SetIntegrationEventUniqueParameter(ChatMessageChangedIntegrationEvent integrationEvent, NewEventChatMessage newEventChatMessage)
    {
        integrationEvent.EventId = newEventChatMessage.EventId;
    }

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
