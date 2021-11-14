using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Team;
using MapsterMapper;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        private readonly IMapper _mapper;

        private readonly IValidator<CreateTeamModel> _createTeamModelValidator;

        private readonly ITeamRepository _teamRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;

        public TeamService(IMapper mapper,
            IValidator<CreateTeamModel> createTeamModelValidator,
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _createTeamModelValidator = createTeamModelValidator;
            _teamRepository = teamRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            await _createTeamModelValidator.ValidateAndThrowAsync(createTeamModel);

            var isEventExist = await _eventRepository.ExistAsync(createTeamModel.EventId);

            if (!isEventExist)
                throw new ServiceException("События с таким идентификатором не существует");

            var isSameNameExist = await _teamRepository.ExistAsync(createTeamModel.Name);

            if (isSameNameExist)
                throw new ServiceException("Команда с таким названием уже существует");

            var eventModel = await _eventRepository.GetAsync(createTeamModel.EventId);

            if (eventModel.Status != EventStatus.Published)
                throw new ServiceException("Зарегистрировать команду возможно только для опубликованного события");

            var createTeamEntity = _mapper.Map<CreateTeamModel>(createTeamModel);
            return await _teamRepository.CreateAsync(createTeamEntity);
        }

        public async Task AddMemberAsync(long teamId, long userId)
        {
            if (teamId <= default(long))
                throw new ServiceException("Идентификатор команды должен быть больше 0");

            if (userId <= default(long))
                throw new ServiceException("Идентификатор пользователя должен быть больше 0");

            if (await _teamRepository.ExistAsync(teamId) == false)
                throw new ServiceException("Команды с таким идентификатором не существует");

            if (await _userRepository.ExistAsync(userId) == false)
                throw new ServiceException("Пользователя с таким идентификатором не существует");

            var teamModel = await _teamRepository.GetAsync(teamId);

            var eventModel = await _eventRepository.GetAsync(teamModel.EventId);

            if (eventModel == null)
                throw new ServiceException("События с таким идентификатором не найдено");

            if (teamModel.Members.Any(x => x.Id == userId))
                throw new ServiceException("Пользователь уже добавлен в эту команду");

            if (eventModel.Status != EventStatus.Published)
                throw new ServiceException("Невозможно добавить участника в команду. Событие не опубликовано.");

            await _teamRepository.AddMemberAsync(teamId, userId);
        }
    }
}