using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Validation.Events;
using Hackathon.BL.Validation.Teams;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Projects;

namespace Hackathon.BL.Validation.Projects;

public class ProjectCreateParametersValidator: Hackathon.Common.Abstraction.IValidator<ProjectCreationParameters>
{
    private readonly IValidator<BaseProjectParameters> _modelValidator;
    private readonly ITeamRepository _teamRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IProjectRepository _projectRepository;

    public ProjectCreateParametersValidator(
        IValidator<BaseProjectParameters> modelValidator,
        ITeamRepository teamRepository,
        IEventRepository eventRepository,
        IProjectRepository projectRepository)
    {
        _modelValidator = modelValidator;
        _teamRepository = teamRepository;
        _eventRepository = eventRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result> ValidateAsync(ProjectCreationParameters model, long? authorizedUserId = null)
    {
        var validationResult = await _modelValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Result.NotValid(validationResult.Errors.First().ErrorMessage);
        }

        var @event = await _eventRepository.GetAsync(model.EventId);
        if (@event is null)
        {
            return Result.NotValid(EventsValidationMessages.EventDoesNotExists);
        }

        if (@event.Status is not EventStatus.Started)
        {
            return Result.NotValid(ProjectValidationErrorMessages.ProjectCanBeCreatedOnlyForStartedEvent);
        }

        var team = await _teamRepository.GetAsync(model.TeamId);
        if (team is null)
        {
            return Result.NotValid(TeamValidationErrorMessages.TeamDoesNotExists);
        }

        if (!team.IsTemporaryTeam())
        {
            if (!authorizedUserId.HasValue || !team.HasOwnerWithId(authorizedUserId.Value))
            {
                return Result.Forbidden("Создавать проект может только владелец команды");
            }
        }
        else
        {
            //TODO: определить ответственного и проверить его идентификатор
        }

        var project = await _projectRepository.GetAsync(model.EventId, model.TeamId);

        return project is not null
            ? Result.NotValid(ProjectValidationErrorMessages.ProjectAlreadyExists)
            : Result.Success;
    }
}
