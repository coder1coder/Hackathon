using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        private readonly IValidator<CreateTeamModel> _createTeamModelValidator;

        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<TeamMemberModel> _teamAddMemberModelValidator;
        private readonly IValidator<GetListModel<TeamFilterModel>> _getFilterModelValidator;

        public TeamService(
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<TeamMemberModel> teamAddMemberModelValidator,
            IValidator<GetListModel<TeamFilterModel>> getFilterModelValidator,
            ITeamRepository teamRepository
            )
        {
            _createTeamModelValidator = createTeamModelValidator;
            _teamAddMemberModelValidator = teamAddMemberModelValidator;
            _getFilterModelValidator = getFilterModelValidator;
            
            _teamRepository = teamRepository;
        }

        /// <inheritdoc cref="ITeamService.CreateAsync(CreateTeamModel)"/>
        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            await _createTeamModelValidator.ValidateAndThrowAsync(createTeamModel);

            // Проверяем, является ли пользователь, который создает команду,
            // владельцем команды которая уже существует
            // В случае, когда команда создается автоматически системой
            // OwnerId = null
            if (createTeamModel.OwnerId.HasValue)
            {
                var teams = await _teamRepository.GetAsync(new GetListModel<TeamFilterModel>
                {
                    Filter = new TeamFilterModel
                    {
                        OwnerId = createTeamModel.OwnerId
                    },
                    Page = 0,
                    PageSize = 1
                });

                if (teams.Items.Any())
                    throw new ValidationException("Пользователь уже является владельцем команды");
            }
            
            return await _teamRepository.CreateAsync(createTeamModel);
        }

        /// <inheritdoc cref="ITeamService.AddMemberAsync(TeamMemberModel)"/>
        public async Task AddMemberAsync(TeamMemberModel teamMemberModel)
        {
            await _teamAddMemberModelValidator.ValidateAndThrowAsync(teamMemberModel);
            await _teamRepository.AddMemberAsync(teamMemberModel);
        }

        /// <inheritdoc cref="ITeamService.GetAsync(long)"/>
        public async Task<TeamModel> GetAsync(long teamId)
        {
            return await _teamRepository.GetAsync(teamId);
        }

        /// <inheritdoc cref="ITeamService.GetAsync(GetListModel{TeamFilterModel})"/>
        public async Task<BaseCollectionModel<TeamModel>> GetAsync(GetListModel<TeamFilterModel> getListModel)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getListModel);
            return await _teamRepository.GetAsync(getListModel);
        }

        /// <inheritdoc cref="ITeamService.GetUserTeam(long)"/>
        public async Task<TeamModel> GetUserTeam(long userId)
            => (await _teamRepository.GetByExpression(x =>
                (x.Users.Any(u => u.Id == userId) && x.OwnerId != null)
                || x.OwnerId == userId)).FirstOrDefault();

        /// <inheritdoc cref="ITeamService.RemoveMemberAsync(TeamMemberModel)"/>
        public async Task RemoveMemberAsync(TeamMemberModel teamMemberModel)
            => await _teamRepository.RemoveMemberAsync(teamMemberModel);
    }
}