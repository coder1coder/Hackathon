using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Project
{
    public class ProjectService: IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<ProjectCreateModel> _projectCreateModelValidator;

        public ProjectService(
            IProjectRepository projectRepository,
            IValidator<ProjectCreateModel> projectCreateModelValidator)
        {
            _projectRepository = projectRepository;
            _projectCreateModelValidator = projectCreateModelValidator;
        }

        /// <inheritdoc cref="IProjecService.CreateAsync(ProjectCreateModel)"/>
        public async Task<long> CreateAsync(ProjectCreateModel projectCreateModel)
        {
            await _projectCreateModelValidator.ValidateAndThrowAsync(projectCreateModel);

            // var team = await teamRepository.GetAsync(teamId);
            //
            // if (team == null)
            // {
            //     context.AddFailure("Команды с указаным идентификатором не существует");
            // }
            // else
            // {
            //     if (Team)
            //         if (team.Project != null)
            //             context.AddFailure("Команда уже имеет проект");
            //
            //     if (team.Event.Status != EventStatus.Published)
            //         context.AddFailure("Невозможно добавить проект пока событие не опубликовано");
            // }

            return await _projectRepository.CreateAsync(projectCreateModel);
        }
    }
}