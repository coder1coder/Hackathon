using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Project;
using Hackathon.Abstraction.Team;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using ValidationException = FluentValidation.ValidationException;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        public const int MAX_TEAM_MEMBERS = 30;

        private readonly IValidator<CreateTeamModel> _createTeamModelValidator;

        private readonly ITeamRepository _teamRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IProjectRepository _projectRepository;

        private readonly IValidator<TeamMemberModel> _teamAddMemberModelValidator;
        private readonly IValidator<GetListParameters<TeamFilter>> _getFilterModelValidator;

        public TeamService(
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<TeamMemberModel> teamAddMemberModelValidator,
            IValidator<GetListParameters<TeamFilter>> getFilterModelValidator,
            ITeamRepository teamRepository, 
            IEventRepository eventRepository, 
            IProjectRepository projectRepository)
        {
            _createTeamModelValidator = createTeamModelValidator;
            _teamAddMemberModelValidator = teamAddMemberModelValidator;
            _getFilterModelValidator = getFilterModelValidator;
            
            _teamRepository = teamRepository;
            _eventRepository = eventRepository;
            _projectRepository = projectRepository;
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
                var teams = await _teamRepository.GetAsync(new GetListParameters<TeamFilter>
                {
                    Filter = new TeamFilter
                    {
                        OwnerId = createTeamModel.OwnerId
                    },
                    Offset = 0,
                    Limit = 1
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
            => await _teamRepository.GetAsync(teamId);

        /// <inheritdoc cref="ITeamService.GetAsync(GetListParameters{TeamFilter})"/>
        public async Task<BaseCollection<TeamModel>> GetAsync(GetListParameters<TeamFilter> getListParameters)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getListParameters);
            return await _teamRepository.GetAsync(getListParameters);
        }

        /// <inheritdoc cref="ITeamService.GetUserTeam(long)"/>
        public async Task<TeamGeneral> GetUserTeam(long userId)
        {
            var teams = await _teamRepository.GetByExpressionAsync(x =>
                (x.Members.Any(u => u.MemberId == userId) && x.OwnerId != null)
                || x.OwnerId == userId);

            if (!teams.Any())
                throw new EntityNotFoundException("Команда не найдена");

            var userTeam = teams.First();
            userTeam.Members.Add(userTeam.Owner);
            
            return new TeamGeneral
            {
                Id = userTeam.Id,
                Name = userTeam.Name,
                Owner = userTeam.Owner,
                Members = userTeam.Members.ToArray()
            };
        }

        /// <inheritdoc cref="ITeamService.RemoveMemberAsync(TeamMemberModel)"/>
        public async Task RemoveMemberAsync(TeamMemberModel teamMemberModel)
        {
            // Определить роль участика. Владелец / участник
            var team = await _teamRepository.GetAsync(teamMemberModel.TeamId);

            var IsOwnerMember = team.OwnerId == teamMemberModel.MemberId;

            // Если пользователь является создателем, то нужно передать права владения группой 
            if (IsOwnerMember)
            {
                if (team.Members != null
                    && team.Members.Count > 0)
                {
                    // кому теперь будет принадлежать команда
                    // пользователь раньше остальных вступил в команду
                    var newOwner = team.Members
                        .OrderByDescending(u => u.DateTimeAdd)
                        .First();

                    ChangeOwnerModel changeOwnerModel = new ChangeOwnerModel()
                    {
                        TeamId = teamMemberModel.TeamId,
                        OwnerId = teamMemberModel.MemberId,
                        NewOwnerId = newOwner.Id
                    };

                    await _teamRepository.ChangeTeamOwnerAsync(changeOwnerModel);
                }
                else
                {
                    // TODO: События команды также удаляем?
                    DeleteTeamModel deleteTeamModel = new DeleteTeamModel()
                    {
                        TeamId = teamMemberModel.TeamId,
                        EventId = null
                    };

                    // TODO: SoftDelete
                    await _teamRepository.DeleteTeamAsync(deleteTeamModel);
                }
            }

            // Если пользователь не создатель, то исключим из команды
            if (!IsOwnerMember)
                await _teamRepository.RemoveMemberAsync(teamMemberModel);
        }

        /// <inheritdoc cref="ITeamService.GetTeamEvents(long, PaginationSort)"/>
        public async Task<BaseCollection<TeamEventListItem>> GetTeamEvents(long teamId, PaginationSort paginationSort)
        {
            var isTeamExists = await _teamRepository.ExistAsync(teamId);

            if (!isTeamExists)
                throw new EntityNotFoundException("Команда не найдена");

            var events = await _eventRepository.GetListAsync(1, new GetListParameters<EventFilter>
            {
                Filter = new EventFilter
                {
                    TeamsIds = new[] { teamId }
                },
                Limit = paginationSort.Limit
            });

            if (events.Items.Count == 0)
                return  new BaseCollection<TeamEventListItem>
                {
                    Items = Array.Empty<TeamEventListItem>(),
                    TotalCount = events.TotalCount
                };

            var eventsIds = events.Items.Select(x => x.Id).ToArray();

            var projects = await _projectRepository.GetListAsync(new GetListParameters<ProjectFilter>
            {
                Filter = new ProjectFilter
                {
                    EventsIds = eventsIds,
                    TeamsIds = new []{ teamId }
                },
                Limit = eventsIds.Length,
                Offset = 0,
                SortBy = paginationSort.SortBy,
                SortOrder = paginationSort.SortOrder
            });

            return new BaseCollection<TeamEventListItem>
            {
                Items = projects.Items.Select(x => new TeamEventListItem
                {
                    EventId = x.EventId,
                    EventName = x.EventName,
                    ProjectId = x.Id,
                    ProjectName = x.Name,
                    TeamId = x.TeamId,
                    TeamName = x.TeamName
                }).ToArray(),
                TotalCount = events.TotalCount
            };
        }
    }
}