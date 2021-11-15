using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using MapsterMapper;

namespace Hackathon.BL.Project
{
    public class ProjectService: IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;

        public ProjectService(
            IMapper mapper,
            IProjectRepository projectRepository,
            ITeamRepository teamRepository
            )
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
        }

        public async Task<long> CreateAsync(ProjectCreateModel projectCreateModel)
        {
            if (string.IsNullOrWhiteSpace(projectCreateModel.Name) || projectCreateModel.Name.Length is < 3 or > 200)
                throw new ServiceException("Название проекта должно содержать от 3 до 200 символов");

            if (projectCreateModel.TeamId <= default(long))
                throw new ServiceException("Идентификатор команды должен быть больше 0");

            var team = await _teamRepository.GetAsync(projectCreateModel.TeamId);

            if (team == null)
                throw new ServiceException("Команды с указаным идентификатором не существует");

            if (team.Project != null)
                throw new ServiceException("Команда уже имеет проект");

            if (team.Event.Status != EventStatus.Published)
                throw new ServiceException("Невозхможно добавить проект пока событие не опубликовано");

            return await _projectRepository.CreateAsync(projectCreateModel);
        }
    }
}