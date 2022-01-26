using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        private readonly IValidator<CreateTeamModel> _createTeamModelValidator;

        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<long> _teamExistValidator;
        private readonly IValidator<TeamAddMemberModel> _teamAddMemberModelValidator;
        private readonly IValidator<GetFilterModel<TeamFilterModel>> _getFilterModelValidator;

        public TeamService(
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<TeamAddMemberModel> teamAddMemberModelValidator,
            IValidator<long> teamExistValidator,
            ITeamRepository teamRepository,
            IValidator<GetFilterModel<TeamFilterModel>> getFilterModelValidator)
        {
            _createTeamModelValidator = createTeamModelValidator;
            _teamAddMemberModelValidator = teamAddMemberModelValidator;
            _teamExistValidator = teamExistValidator;

            _teamRepository = teamRepository;
            _getFilterModelValidator = getFilterModelValidator;
        }

        /// <inheritdoc cref="ITeamService.CreateAsync(CreateTeamModel)"/>
        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            await _createTeamModelValidator.ValidateAndThrowAsync(createTeamModel);
            return await _teamRepository.CreateAsync(createTeamModel);
        }

        /// <inheritdoc cref="ITeamService.AddMemberAsync(TeamAddMemberModel)"/>
        public async Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel)
        {
            await _teamAddMemberModelValidator.ValidateAndThrowAsync(teamAddMemberModel);
            await _teamRepository.AddMemberAsync(teamAddMemberModel);
        }

        /// <inheritdoc cref="ITeamService.GetAsync(long)"/>
        public async Task<TeamModel> GetAsync(long teamId)
        {
            await _teamExistValidator.ValidateAndThrowAsync(teamId);
            return await _teamRepository.GetAsync(teamId);
        }

        /// <inheritdoc cref="ITeamService.GetAsync(GetFilterModel{TeamFilterModel})"/>
        public async Task<BaseCollectionModel<TeamModel>> GetAsync(GetFilterModel<TeamFilterModel> getFilterModel)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getFilterModel);
            return await _teamRepository.GetAsync(getFilterModel);
        }
    }
}