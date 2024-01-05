using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.BL.Validation.Events;
using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Models.Events;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Models.Event;

namespace Hackathon.Chats.BL.Validators;

public class NewEventChatMessageValidator: IValidator<NewEventChatMessage>
{
    private readonly FluentValidation.IValidator<INewChatMessage> _modelValidator;
    private readonly IEventRepository _eventRepository;

    public NewEventChatMessageValidator(FluentValidation.IValidator<INewChatMessage> modelValidator, IEventRepository eventRepository)
    {
        _modelValidator = modelValidator;
        _eventRepository = eventRepository;
    }

    public async Task<Result> ValidateAsync(NewEventChatMessage message, long? authorizedUserId = null)
    {
        var validationResult = await _modelValidator.ValidateAsync(message);

        if (!validationResult.IsValid)
            return Result.NotValid(validationResult.Errors.First().ErrorMessage);

        var @event = await _eventRepository.GetAsync(message.EventId);
        if (@event is null)
            return Result.NotValid(EventsValidationMessages.EventDoesNotExists);

        return @event.Status is not EventStatus.Started
            ? Result.NotValid(ChatErrorMessages.MessageCanBeSentOnlyForStartedEvent)
            : Result.Success;
    }
}
