using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction;
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
        private readonly IValidator<GetListModel<TeamFilterModel>> _getFilterModelValidator;

        public TeamService(
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<TeamAddMemberModel> teamAddMemberModelValidator,
            IValidator<long> teamExistValidator,
            ITeamRepository teamRepository,
            IValidator<GetListModel<TeamFilterModel>> getFilterModelValidator)
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

        /// <inheritdoc cref="ITeamService.GetAsync(GetListModel{T})"/>
        public async Task<BaseCollectionModel<TeamModel>> GetAsync(GetListModel<TeamFilterModel> getListModel)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getListModel);
            return await _teamRepository.GetAsync(getListModel);
        }
    }
}