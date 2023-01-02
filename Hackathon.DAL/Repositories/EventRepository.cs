using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Abstraction.Event;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Hackathon.DAL.Repositories
{
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

            //TODO: разобраться (падают тесты)
            _dbContext.ChangeTracker.Clear();
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
                .SingleOrDefaultAsync(x => x.Id == eventId);

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

                if (parameters.Filter.ExcludeOtherUsersDraftedEvents)
                    query = query.Where(x =>
                        !(x.OwnerId != userId && x.Status == EventStatus.Draft));

                if (parameters.Filter.StartFrom.HasValue)
                {
                    var startFrom = parameters.Filter.StartFrom.Value.ToUtcWithoutSeconds();
                    query = query.Where(x => x.Start.Date >= startFrom);
                }

                if (parameters.Filter.StartTo.HasValue)
                {
                    var startTo = parameters.Filter.StartTo.Value.ToUtcWithoutSeconds();
                    query = query.Where(x => x.Start <= startTo);
                }

                if (parameters.Filter.TeamsIds is not null)
                    query = query.Where(x =>
                        x.Teams.Any(t =>
                            parameters.Filter.TeamsIds.Contains(t.Id)));

                if (parameters.Filter.OwnerIds is not null)
                    query = query.Where(x =>
                        parameters.Filter.OwnerIds.Contains(x.OwnerId));
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                query = parameters.SortBy switch
                {
                    nameof(EventEntity.Name) => parameters.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name),

                    nameof(EventEntity.Start) => parameters.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Start)
                        : query.OrderByDescending(x => x.Start),

                    nameof(EventEntity.Status) => parameters.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Status)
                        : query.OrderByDescending(x => x.Status),

                    _ => parameters.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

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
                    OwnerName = x.Owner.FullName ?? x.Owner.UserName,
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

        public async Task<EventModel[]> GetByExpression(Expression<Func<EventEntity, bool>> expression)
        {
            var entities = await _dbContext.Events.Where(expression)
                .Where(expression)
                .ToArrayAsync();

            return _mapper.Map<EventEntity[], EventModel[]>(entities);
        }

        public async Task UpdateAsync(EventUpdateParameters eventUpdateParameters)
        {
            var entity = await _dbContext.Events
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
            var eventEntity = await _dbContext.Events.SingleOrDefaultAsync(x => x.Id == eventId);

            if (eventEntity == null)
                throw new EntityNotFoundException("Событие с указанным идентификатором не найдено");

            eventEntity.Status = eventStatus;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(long eventId)
        {
            var eventEntity = await _dbContext.Events.SingleOrDefaultAsync(x => x.Id == eventId);

            if (eventEntity == null)
                throw new EntityNotFoundException("Событие с указанным идентификатором не найдено");

            eventEntity.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        public Task<bool> IsExists(long eventId)
            => _dbContext.Events.AnyAsync(x =>
                x.Id == eventId
                && !x.IsDeleted);
    }
}
