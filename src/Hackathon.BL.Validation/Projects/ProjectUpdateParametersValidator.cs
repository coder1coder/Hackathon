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

public class ProjectUpdateParametersValidator: Hackathon.Common.Abstraction.IValidator<ProjectUpdateParameters>
{
    private readonly IValidator<BaseProjectParameters> _modelValidator;
    private readonly IEventRepository _eventRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IProjectRepository _projectRepository;

    public ProjectUpdateParametersValidator(
        IValidator<BaseProjectParameters> modelValidator,
        IEventRepository eventRepository,
        ITeamRepository teamRepository,
        IProjectRepository projectRepository)
    {
        _modelValidator = modelValidator;
        _eventRepository = eventRepository;
        _teamRepository = teamRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result> ValidateAsync(ProjectUpdateParameters parameters, long? authorizedUserId = null)
    {
        var validationResult = await _modelValidator.ValidateAsync(parameters);

        if (!validationResult.IsValid)
        {
            return Result.NotValid(validationResult.Errors.First().ErrorMessage);
        }

        var @event = await _eventRepository.GetAsync(parameters.EventId);
        if (@event is null)
        {
            return Result.NotValid(EventsValidationMessages.EventDoesNotExists);
        }

        if (@event.Status is not EventStatus.Started)
        {
            return Result.NotValid(ProjectValidationErrorMessages.ProjectCanBeCreatedOnlyForStartedEvent);
        }

        var team = await _teamRepository.GetAsync(parameters.TeamId);
        if (team is null)
        {
            return Result.NotValid(TeamValidationErrorMessages.TeamDoesNotExists);
        }

        if (!team.IsTemporaryTeam())
        {
            if (!authorizedUserId.HasValue || !team.HasOwnerWithId(authorizedUserId.Value))
            {
                return Result.Forbidden("Обновлять проект может только владелец команды");
            }
        }
        else
        {
            //TODO: определить ответственного и проверить его идентификатор
        }

        var project = await _projectRepository.GetAsync(parameters.EventId, parameters.TeamId);

        return project is null
            ? Result.NotFound(ProjectValidationErrorMessages.ProjectDoesNotExist)
            : Result.Success;
    }
}
