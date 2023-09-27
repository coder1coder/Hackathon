using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Event;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MapsterMapper;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Notifications;

namespace Hackathon.BL.Chat;

public class EventBaseChatService: BaseChatService<NewEventChatMessage, EventChatMessage>, IEventChatService
{
    private readonly IEventRepository _eventRepository;
    private readonly Common.Abstraction.IValidator<NewEventChatMessage> _validator;

    public EventBaseChatService(
        IEventChatRepository eventChatRepository,
        IEventRepository eventRepository,
        IMessageHub<ChatMessageChangedIntegrationEvent> chatMessageHub,
        IUserRepository userRepository,
        INotificationService notificationService,
        Common.Abstraction.IValidator<NewEventChatMessage> validator,
        IMapper mapper):base(eventChatRepository, chatMessageHub, userRepository, notificationService, mapper)
    {
        _eventRepository = eventRepository;
        _validator = validator;
    }

    protected override Task<Result> ValidateAsync(NewEventChatMessage message)
        => _validator.ValidateAsync(message);

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
