using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Validation.User;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Events;

namespace Hackathon.BL.Team;

public class TeamService : ITeamService
{
    private readonly IValidator<CreateTeamModel> _createTeamModelValidator;

    private readonly ITeamRepository _teamRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    private readonly IValidator<TeamMemberModel> _teamAddMemberModelValidator;
    private readonly IValidator<Common.Models.GetListParameters<TeamFilter>> _getFilterModelValidator;

    public TeamService(
        IValidator<CreateTeamModel> createTeamModelValidator,
        IValidator<TeamMemberModel> teamAddMemberModelValidator,
        IValidator<Common.Models.GetListParameters<TeamFilter>> getFilterModelValidator,
        ITeamRepository teamRepository,
        IEventRepository eventRepository,
        IProjectRepository projectRepository,
        IUserRepository userRepository)
    {
        _createTeamModelValidator = createTeamModelValidator;
        _teamAddMemberModelValidator = teamAddMemberModelValidator;
        _getFilterModelValidator = getFilterModelValidator;

        _teamRepository = teamRepository;
        _eventRepository = eventRepository;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<long>> CreateAsync(CreateTeamModel createTeamModel, long? userId = null)
    {
        createTeamModel.OwnerId = userId;

        await _createTeamModelValidator.ValidateAndThrowAsync(createTeamModel);

        if (createTeamModel.EventId.HasValue && userId.HasValue)
        {
            var eventModel = await _eventRepository.GetAsync(createTeamModel.EventId.Value);
            if (eventModel is null)
                return Result<long>.NotValid(EventMessages.EventNotFound);

            if (eventModel.Owner.Id != userId)
                return Result<long>.Forbidden(TeamMessages.CreateTeamAccessDenied);
        }

        // Проверяем, является ли пользователь, который создает команду,
        // владельцем команды которая уже существует
        // В случае, когда команда создается автоматически системой
        // OwnerId = null
        if (createTeamModel.OwnerId.HasValue)
        {
            var teams = await _teamRepository.GetListAsync(new Common.Models.GetListParameters<TeamFilter>
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
                return Result<long>.NotValid(TeamMessages.UserAlreadyOwnerOfTeam);
        }

        var newTeamId = await _teamRepository.CreateAsync(createTeamModel);

        return Result<long>.FromValue(newTeamId);
    }

    public async Task<Result> AddMemberAsync(TeamMemberModel teamMemberModel)
    {
        await _teamAddMemberModelValidator.ValidateAndThrowAsync(teamMemberModel);

        var teamExists = await _teamRepository.ExistAsync(teamMemberModel.TeamId);

        if (!teamExists)
            return Result.NotValid(TeamMessages.TeamDoesNotExists);

        var userExists = await _userRepository.ExistsAsync(teamMemberModel.MemberId);

        if (!userExists)
            return Result.NotValid(UserErrorMessages.UserDoesNotExists);

        await _teamRepository.AddMemberAsync(teamMemberModel);

        return Result.Success;
    }

    public async Task<Result<TeamModel>> GetAsync(long teamId)
        => Result<TeamModel>.FromValue(await _teamRepository.GetAsync(teamId));

    public async Task<BaseCollection<TeamModel>> GetListAsync(Common.Models.GetListParameters<TeamFilter> getListParameters)
    {
        await _getFilterModelValidator.ValidateAndThrowAsync(getListParameters);
        return await _teamRepository.GetListAsync(getListParameters);
    }

    public async Task<Result<TeamGeneral>> GetUserTeam(long userId)
    {
        var teams = await _teamRepository.GetListAsync(new Common.Models.GetListParameters<TeamFilter>
        {
            Filter = new TeamFilter
            {
                MemberId = userId,
                HasOwner = true
            },
            Limit = 1
        });

        if (teams?.Items is not {Count: > 0})
        {
            return Result<TeamGeneral>.NotFound(TeamMessages.TeamDoesNotExists);
        }

        var userTeam = teams.Items.First();

        return Result<TeamGeneral>.FromValue(new TeamGeneral
        {
            Id = userTeam.Id,
            Name = userTeam.Name,
            Owner = userTeam.Owner,
            Members = userTeam.Members
        });
    }

    //TODO: написать тесты
    public async Task<Result> RemoveMemberAsync(TeamMemberModel teamMemberModel)
    {
        // Определить роль участика. Владелец / участник
        var team = await _teamRepository.GetAsync(teamMemberModel.TeamId);

        if (team is null)
            return Result.NotValid(TeamMessages.TeamDoesNotExists);

        var userExists = await _userRepository.ExistsAsync(teamMemberModel.MemberId);

        if (!userExists)
            return Result.NotValid(UserErrorMessages.UserDoesNotExists);

        if (!team.HasMemberWithId(teamMemberModel.MemberId))
            return Result.NotValid(TeamMessages.UserIsNotOnTeam);

        if (team.HasOwnerWithId(teamMemberModel.MemberId))
        {
            await RemoveOwnerAsync(team);
        }
        else
        {
            await _teamRepository.RemoveMemberAsync(teamMemberModel);
        }

        return Result.Success;
    }

    public async Task<Result<BaseCollection<TeamEventListItem>>> GetTeamEvents(long teamId, PaginationSort paginationSort)
    {
        var isTeamExists = await _teamRepository.ExistAsync(teamId);

        if (!isTeamExists)
            Result<BaseCollection<TeamEventListItem>>.NotFound(TeamMessages.TeamDoesNotExists);

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
                ProjectName = x.Name,
                TeamId = x.TeamId,
                TeamName = x.TeamName
            }).ToArray(),
            TotalCount = events.TotalCount
        });
    }

    /// <summary>
    /// Исключить владельца команды из неё
    /// </summary>
    /// <param name="team">Модель команды</param>
    private async Task RemoveOwnerAsync(TeamModel team)
    {
        var membersWhoNotOwner = team.Members?.Where(x => x.Id != team.OwnerId).ToArray();

        if (membersWhoNotOwner is not {Length: > 0})
        {
            await _teamRepository.DeleteTeamAsync(new DeleteTeamModel
            {
                TeamId = team.Id,
                EventId = null
            });
            return;
        }

        // кому теперь будет принадлежать команда
        // пользователь раньше остальных вступил в команду
        var newOwner = membersWhoNotOwner
            .OrderByDescending(u => u.DateTimeAdd)
            .First();

        team.OwnerId = newOwner.Id;
        await _teamRepository.SetOwnerAsync(team.Id, newOwner.Id);
    }
}
