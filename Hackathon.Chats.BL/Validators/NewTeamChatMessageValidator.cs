using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.BL.Validation.Event;
using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Event;

namespace Hackathon.Chats.BL.Validators;

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
