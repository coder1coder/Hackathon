using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Project;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.BL.Validation.Team;
using Hackathon.BL.Validation.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using MapsterMapper;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        private const string UserAlreadyOwnerOfTeam = "Пользователь уже является владельцем команды";
        private const string UserIsNotOnTeam = "Пользователь не состоит в команде";
        public const string CreateTeamAccessDenied = "Создать команду для события может только владелец события";

        private readonly IValidator<CreateTeamModel> _createTeamModelValidator;

        private readonly ITeamRepository _teamRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        private readonly IValidator<TeamMemberModel> _teamAddMemberModelValidator;
        private readonly IValidator<Common.Models.GetListParameters<TeamFilter>> _getFilterModelValidator;

        private readonly IMapper _mapper;

        public TeamService(
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<TeamMemberModel> teamAddMemberModelValidator,
            IValidator<Common.Models.GetListParameters<TeamFilter>> getFilterModelValidator,
            ITeamRepository teamRepository,
            IEventRepository eventRepository,
            IProjectRepository projectRepository,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _createTeamModelValidator = createTeamModelValidator;
            _teamAddMemberModelValidator = teamAddMemberModelValidator;
            _getFilterModelValidator = getFilterModelValidator;

            _teamRepository = teamRepository;
            _eventRepository = eventRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<long> CreateAsync(CreateTeamModel createTeamModel, long? userId = null)
        {
            createTeamModel.OwnerId = userId;

            await _createTeamModelValidator.ValidateAndThrowAsync(createTeamModel);

            if (createTeamModel.EventId.HasValue && userId.HasValue)
            {
                var eventModel = await _eventRepository.GetAsync(createTeamModel.EventId.Value);
                if (eventModel is null)
                {
                    throw new ValidationException("Событие не существует");
                }

                if (eventModel.Owner.Id != userId)
                {
                    throw new ValidationException(CreateTeamAccessDenied);
                }
            }

            // Проверяем, является ли пользователь, который создает команду,
            // владельцем команды которая уже существует
            // В случае, когда команда создается автоматически системой
            // OwnerId = null
            if (createTeamModel.OwnerId.HasValue)
            {
                var teams = await _teamRepository.GetAsync(new Common.Models.GetListParameters<TeamFilter>
                {
                    Filter = new TeamFilter
                    {
                        EventId = createTeamModel.EventId,
                        OwnerId = createTeamModel.OwnerId
                    },
                    Offset = 0,
                    Limit = 1
                });

                if (teams.Items.Any())
                    throw new ValidationException(UserAlreadyOwnerOfTeam);
            }

            return await _teamRepository.CreateAsync(createTeamModel);
        }

        public async Task AddMemberAsync(TeamMemberModel teamMemberModel)
        {
            await _teamAddMemberModelValidator.ValidateAndThrowAsync(teamMemberModel);

            var teamExists = await _teamRepository.ExistAsync(teamMemberModel.TeamId);

            if (!teamExists)
                throw new ValidationException(TeamErrorMessages.TeamDoesNotExists);

            var userExists = await _userRepository.ExistsAsync(teamMemberModel.MemberId);

            if (!userExists)
                throw new ValidationException(UserErrorMessages.UserDoesNotExists);

            await _teamRepository.AddMemberAsync(teamMemberModel);
        }

        public async Task<Result<TeamModel>> GetAsync(long teamId)
            => Result<TeamModel>.FromValue(await _teamRepository.GetAsync(teamId));

        public async Task<BaseCollection<TeamModel>> GetAsync(Common.Models.GetListParameters<TeamFilter> getListParameters)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getListParameters);
            return await _teamRepository.GetAsync(getListParameters);
        }

        public async Task<Result<TeamGeneral>> GetUserTeam(long userId)
        {
            var teams = await _teamRepository.GetByExpressionAsync(x =>
               x.OwnerId != null && (x.OwnerId == userId || x.Members.Any(s => s.MemberId == userId)));

            if (!teams.Any())
                return Result<TeamGeneral>.NotFound(TeamErrorMessages.TeamDoesNotExists);

            var userTeam = teams.First();
            userTeam.Members = new List<UserModel>(userTeam.Members) {userTeam.Owner}.ToArray();

            return Result<TeamGeneral>.FromValue(new TeamGeneral
            {
                Id = userTeam.Id,
                Name = userTeam.Name,
                Owner = _mapper.Map<UserModel, UserShortModel>(userTeam.Owner),
                Members = _mapper.Map<UserModel[], UserShortModel[]>(userTeam.Members)
            });
        }

        public async Task RemoveMemberAsync(TeamMemberModel teamMemberModel)
        {
            // Определить роль участика. Владелец / участник
            var team = await _teamRepository.GetAsync(teamMemberModel.TeamId);

            if (team is null)
                throw new ValidationException(TeamErrorMessages.TeamDoesNotExists);

            var userExists = await _userRepository.ExistsAsync(teamMemberModel.MemberId);

            if (!userExists)
                throw new ValidationException(UserErrorMessages.UserDoesNotExists);

            var teamMember = team.Members.FirstOrDefault(x => x.Id == teamMemberModel.MemberId);

            if (teamMember is null && team.OwnerId != teamMemberModel.MemberId)
                throw new ValidationException(UserIsNotOnTeam);

            var isOwnerMember = team.OwnerId == teamMemberModel.MemberId;

            // Если пользователь является создателем, то нужно передать права владения группой
            if (isOwnerMember)
            {
                if (team.Members is {Length: > 0})
                {
                    // кому теперь будет принадлежать команда
                    // пользователь раньше остальных вступил в команду
                    var newOwner = team.Members
                        .OrderByDescending(u => u.DateTimeAdd)
                        .First();

                    var changeOwnerModel = new ChangeOwnerModel
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
                    var deleteTeamModel = new DeleteTeamModel
                    {
                        TeamId = teamMemberModel.TeamId,
                        EventId = null
                    };

                    await _teamRepository.DeleteTeamAsync(deleteTeamModel);
                }
            }
            else // Если пользователь не создатель, то исключим из команды
            {
                await _teamRepository.RemoveMemberAsync(teamMemberModel);
            }
        }

        public async Task<Result<BaseCollection<TeamEventListItem>>> GetTeamEvents(long teamId, PaginationSort paginationSort)
        {
            var isTeamExists = await _teamRepository.ExistAsync(teamId);

            if (!isTeamExists)
                Result<BaseCollection<TeamEventListItem>>.NotFound(TeamErrorMessages.TeamDoesNotExists);

            var events = await _eventRepository.GetListAsync(1, new Common.Models.GetListParameters<EventFilter>
            {
                Filter = new EventFilter
                {
                    TeamsIds = new[] { teamId }
                },
                Limit = paginationSort.Limit
            });

            if (events.Items.Count == 0)
                return Result<BaseCollection<TeamEventListItem>>.FromValue(new BaseCollection<TeamEventListItem>
                {
                    Items = Array.Empty<TeamEventListItem>(),
                    TotalCount = events.TotalCount
                });

            var eventsIds = events.Items.Select(x => x.Id).ToArray();

            var projects = await _projectRepository.GetListAsync(new Common.Models.GetListParameters<ProjectFilter>
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

            return Result<BaseCollection<TeamEventListItem>>.FromValue(new BaseCollection<TeamEventListItem>
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
            });
        }
    }
}
