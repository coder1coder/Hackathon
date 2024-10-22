using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Projects;
using Hackathon.DAL.Entities;
using Hackathon.DAL.Entities.Projects;
using Hackathon.DAL.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class ProjectRepository: IProjectRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(
        IMapper mapper,
        ApplicationDbContext dbContext
    )
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task CreateAsync(ProjectCreationParameters projectCreationParameters)
    {
        var projectEntity = _mapper.Map<ProjectEntity>(projectCreationParameters);
        await _dbContext.Projects.AddAsync(projectEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectUpdateParameters parameters)
    {
        var entity = await _dbContext.Projects.FirstOrDefaultAsync(x =>
            x.EventId == parameters.EventId
            && x.TeamId == parameters.TeamId);

        if (entity is not null)
        {
            _mapper.Map(parameters, entity);
            _dbContext.Projects.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateUploadingFromGitInfo(ProjectUploadingFromGitInfoDto uploadingFromGitInfo)
    {
        var entity = await _dbContext.Projects.FirstOrDefaultAsync(x =>
            x.EventId == uploadingFromGitInfo.EventId
            && x.TeamId == uploadingFromGitInfo.TeamId);

        if (entity is not null)
        {
            entity.LinkToGitBranch = uploadingFromGitInfo.LinkToGitBranch;
            entity.FileIds = uploadingFromGitInfo.FileIds;

            _dbContext.Projects.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<BaseCollection<ProjectListItem>> GetListAsync(GetListParameters<ProjectFilter> parameters)
    {
        var query = _dbContext.Projects
            .Include(x => x.Event)
            .Include(x => x.Team)
            .AsNoTracking()
            .AsQueryable();

        if (parameters.Filter?.EventsIds != null)
        {
            query = query.Where(x=>
                parameters.Filter.EventsIds.Contains(x.EventId));
        }

        if (parameters.Filter?.TeamsIds != null)
        {
            query = query.Where(x=>
                parameters.Filter.TeamsIds.Contains(x.TeamId));
        }

        query = parameters.SortOrder == SortOrder.Asc
            ? query.OrderBy(ResolveOrderFieldExpression(parameters))
            : query.OrderByDescending(ResolveOrderFieldExpression(parameters));

        var totalCount = await query.CountAsync();

        return new BaseCollection<ProjectListItem>
        {
            Items = await query
                .Skip(parameters.Offset)
                .Take(parameters.Limit)
                .Select(x=> new ProjectListItem
                {
                    Name = x.Name,
                    EventId = x.EventId,
                    TeamId = x.TeamId,
                    EventName = x.Event.Name,
                    TeamName = x.Team.Name
                })
                .ToArrayAsync(),
            TotalCount = totalCount
        };
    }

    public async Task<ProjectModel> GetAsync(long eventId, long teamId, bool includeTeamMembers = false)
    {
        var entity = await _dbContext.Projects
            .Include(x=>x.Team)
            .ConditionalThenInclude(x=>x
                .ThenInclude(s=>s.Members)
                .ThenInclude(s=>s.Member), includeTeamMembers)
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.EventId == eventId
                && x.TeamId == teamId);

        return _mapper.Map<ProjectEntity, ProjectModel>(entity);
    }

    public async Task DeleteAsync(long eventId, long teamId)
    {
        var entity = await _dbContext.Projects
            .FirstOrDefaultAsync(x =>
                x.EventId == eventId
                && x.TeamId == teamId);

        if (entity is not null)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    private static Expression<Func<ProjectEntity, object>> ResolveOrderFieldExpression(PaginationSort parameters)
    {
        var availableFields = new Dictionary<string, Expression<Func<ProjectEntity, object>>>
        {
            { nameof(ProjectEntity.EventId).ToLowerInvariant(), x => x.EventId },
            { nameof(ProjectEntity.TeamId).ToLowerInvariant(), x => x.TeamId }
        };

        return !string.IsNullOrWhiteSpace(parameters.SortBy)
               && availableFields.TryGetValue(parameters.SortBy.ToLowerInvariant(), out var sortingDelegate)
            ? sortingDelegate
            : x => x.Name;
    }
}
