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

        public TeamService(IMapper mapper,
            IValidator<CreateTeamModel> createTeamModelValidator,
            ITeamRepository teamRepository,
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _createTeamModelValidator = createTeamModelValidator;
            _teamRepository = teamRepository;
            _eventRepository = eventRepository;
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
    }
}