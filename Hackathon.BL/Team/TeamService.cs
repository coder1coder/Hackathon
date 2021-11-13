using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;
using MapsterMapper;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<TeamModel> _teamModelValidator;
        private readonly ITeamRepository _teamRepository;

        public TeamService(IMapper mapper, IValidator<TeamModel> teamModelValidator,
            ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _teamModelValidator = teamModelValidator;
            _teamRepository = teamRepository;
        }

        public async Task<long> CreateAsync(TeamModel teamModel)
        {
            await _teamModelValidator.ValidateAndThrowAsync(teamModel);
            var canCreate = await _teamRepository.CanCreateAsync(teamModel);
            if (!canCreate)
                throw new ServiceException("Невозможно создать команду");
            var teamEntity = _mapper.Map<TeamEntity>(teamModel);

            return await _teamRepository.CreateAsync(teamEntity);
        }
    }
}