using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.DAL.Entities.Event;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories.Events;

public class EventRepository : IEventRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;

    public EventRepository(
        IMapper mapper,
        ApplicationDbContext dbContext
    )
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(EventCreateParameters eventCreateUpdateParameters)
    {
        var eventEntity = _mapper.Map<EventEntity>(eventCreateUpdateParameters);

        await _dbContext.AddAsync(eventEntity);
        await _dbContext.SaveChangesAsync();

        return eventEntity.Id;
    }

    public async Task<EventModel> GetAsync(long eventId)
    {
        var eventEntity = await _dbContext.Events
            .AsNoTracking()
            .Include(x => x.Teams)
            .ThenInclude(x => x.Members)
            .ThenInclude(x => x.Member)
            .Include(x => x.Owner)
            .Include(x=>x.Stages)
            .Include(x=>x.ApprovalApplication)
            .Include(x=>x.Agreement).ThenInclude(x=>x.Users)
            .FirstOrDefaultAsync(x => x.Id == eventId);

        return eventEntity == null ? null : _mapper.Map<EventModel>(eventEntity);
    }

    public async Task<BaseCollection<EventListItem>> GetListAsync(long userId, GetListParameters<EventFilter> parameters)
    {
        var query = _dbContext.Events
            .AsNoTracking();

        if (parameters.Filter is not null)
        {
            if (parameters.Filter.Ids is not null)
                query = query.Where(x => parameters.Filter.Ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(parameters.Filter.Name))
                query = query.Where(x => x.Name.ToLower().Contains(parameters.Filter.Name.ToLower()));

            if (parameters.Filter.Statuses is not null)
                query = query.Where(x => parameters.Filter.Statuses.Contains(x.Status));

            if (parameters.Filter.ExcludeOtherUsersEventsByStatuses is { Length: > 0 })
            {
                query = query.Where(x => !(x.OwnerId != userId
                                           && parameters.Filter.ExcludeOtherUsersEventsByStatuses.Contains(x.Status)));
            }

            if (parameters.Filter.StartFrom.HasValue)
            {
                var startFrom = parameters.Filter.StartFrom.Value.ToUtcWithoutSeconds();
                query = query.Where(x => x.Start.Date >= startFrom);
            }

            if (parameters.Filter.StartTo.HasValue)
            {
                var startTo = parameters.Filter.StartTo.Value.ToUtcWithoutSeconds();
                query = query.Where(x => x.Start < startTo);
            }

            if (parameters.Filter.TeamsIds is not null)
                query = query.Where(x =>
                    x.Teams.Any(t =>
                        parameters.Filter.TeamsIds.Contains(t.Id)));

            if (parameters.Filter.OwnerIds is not null)
                query = query.Where(x =>
                    parameters.Filter.OwnerIds.Contains(x.OwnerId));
        }

        var totalCount = await query.CountAsync();

        query = parameters.SortOrder == SortOrder.Asc
            ? query.OrderBy(ResolveOrderFieldExpression(parameters))
            : query.OrderByDescending(ResolveOrderFieldExpression(parameters));

        var items = await query
            .Include(x=>x.Teams)
            .ThenInclude(x=>x.Members)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.OwnerId,
                x.Description,
                x.Start,
                x.Status,
                x.MaxEventMembers,
                x.MinTeamMembers,
                x.IsCreateTeamsAutomatically,
                x.ImageId,
                OwnerName = x.Owner.GetAnyName(),
                TeamsCount = x.Teams.Count,
                MembersCount = x.Teams.Sum(z => z.Members.Count)
            })
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ProjectToType<EventListItem>()
            .ToArrayAsync();

        return new BaseCollection<EventListItem>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task UpdateAsync(EventUpdateParameters eventUpdateParameters)
    {
        var entity = await _dbContext.Events
            .Include(x=>x.Stages)
            .Include(x=>x.Agreement)
            .Include(x=>x.ApprovalApplication)
            .FirstOrDefaultAsync(x =>
                x.Id == eventUpdateParameters.Id);

        if (entity is null)
            return;

        _mapper.Map(eventUpdateParameters, entity);

        _dbContext.Events.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
    {
        var eventEntity = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

        if (eventEntity is not null)
        {
            eventEntity.Status = eventStatus;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(long eventId)
    {
        var eventEntity = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);

        if (eventEntity is not null)
        {
            eventEntity.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task SetCurrentStageId(long eventId, long stageId)
    {
        var eventEntity = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);
        if (eventEntity is not null)
        {
            eventEntity.CurrentStageId = stageId;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<EventModel> GetByTemporaryTeamIdAsync(long temporaryTeamId)
    {
        var team = await _dbContext.Teams.AsNoTracking()
            .Include(x => x.Events)
            .FirstOrDefaultAsync(x =>
                x.Id == temporaryTeamId
                && !x.OwnerId.HasValue
                && !x.IsDeleted);

        var eventEntity = team?.Events?.FirstOrDefault();

        return eventEntity == null ? null : _mapper.Map<EventModel>(eventEntity);
    }

    private static Expression<Func<EventEntity, object>> ResolveOrderFieldExpression(PaginationSort parameters)
        => parameters.SortBy switch
        {
            nameof(EventEntity.Name) => x => x.Name,
            nameof(EventEntity.Start) => x => x.Start,
            nameof(EventEntity.Status) => x => x.Status,
            _ => x => x.Id
        };
}
