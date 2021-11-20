using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        private readonly IValidator<CreateTeamModel> _createTeamModelValidator;

        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<long> _teamExistValidator;
        private readonly IValidator<TeamAddMemberModel> _teamAddMemberModelValidator;

        public TeamService(
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<TeamAddMemberModel> teamAddMemberModelValidator,
            IValidator<long> teamExistValidator,
            ITeamRepository teamRepository)
        {
            _createTeamModelValidator = createTeamModelValidator;
            _teamAddMemberModelValidator = teamAddMemberModelValidator;
            _teamExistValidator = teamExistValidator;

            _teamRepository = teamRepository;
        }

        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            await _createTeamModelValidator.ValidateAndThrowAsync(createTeamModel);
            return await _teamRepository.CreateAsync(createTeamModel);
        }

        public async Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel)
        {
            await _teamAddMemberModelValidator.ValidateAndThrowAsync(teamAddMemberModel);
            await _teamRepository.AddMemberAsync(teamAddMemberModel);
        }

        public async Task<TeamModel> GetAsync(long teamId)
        {
            await _teamExistValidator.ValidateAndThrowAsync(teamId);
            return await _teamRepository.GetAsync(teamId);
        }
    }
}