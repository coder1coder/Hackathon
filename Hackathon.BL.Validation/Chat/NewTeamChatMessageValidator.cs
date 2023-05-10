using BackendTools.Common.Models;
using Hackathon.BL.Validation.Event;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Team;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Chat;

public class NewTeamChatMessageValidator: IValidator<NewTeamChatMessage>
{
    private readonly FluentValidation.IValidator<INewChatMessage> _modelValidator;
    private readonly ITeamRepository _teamRepository;
    private readonly IEventRepository _eventRepository;

    public NewTeamChatMessageValidator(
        FluentValidation.IValidator<INewChatMessage> modelValidator,
        ITeamRepository teamRepository,
        IEventRepository eventRepository)
    {
        _modelValidator = modelValidator;
        _teamRepository = teamRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Result> ValidateAsync(NewTeamChatMessage model, long? authorizedUserId = null)
    {
        var validationResult = await _modelValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return Result.NotValid(validationResult.Errors.First().ErrorMessage);

        var team = await _teamRepository.GetAsync(model.TeamId);
        if (team is null)
            return Result.NotValid(TeamMessages.TeamDoesNotExists);

        if (team.IsTemporaryTeam())
        {
            var @event = await _eventRepository.GetByTemporaryTeamIdAsync(team.Id);

            if (@event is null)
                return Result.NotValid(EventErrorMessages.EventDoesNotExists);

            if (@event.Status is not EventStatus.Started)
                return Result.NotValid(ChatErrorMessages.MessageCanBeSentOnlyForStartedEvent);
        }

        return Result.Success;
    }
}
