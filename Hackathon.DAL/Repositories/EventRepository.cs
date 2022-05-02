using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Abstraction.Entities;
using Hackathon.Abstraction.Event;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

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

        /// <inheritdoc cref="IEventRepository.CreateAsync(CreateEventModel)"/>
        public async Task<long> CreateAsync(CreateEventModel createUpdateEventModel)
        {
            var eventEntity = _mapper.Map<EventEntity>(createUpdateEventModel);

            await _dbContext.AddAsync(eventEntity);
            await _dbContext.SaveChangesAsync();

            return eventEntity.Id;
        }

        /// <inheritdoc cref="IEventRepository.GetAsync(long)"/>
        public async Task<EventModel> GetAsync(long eventId)
        {
            var eventEntity = await _dbContext.Events
                .AsNoTracking()
                .Include(x => x.TeamEvents)
                    .ThenInclude(x => x.Team)
                        .ThenInclude(x=>x.Users)
                .Include(x => x.TeamEvents)
                    .ThenInclude(x => x.Project)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == eventId);

            return _mapper.Map<EventModel>(eventEntity);
        }

        /// <inheritdoc cref="IEventRepository.GetAsync(long, GetListModel{EventFilterModel})"/>
        public async Task<BaseCollectionModel<EventModel>> GetAsync(long userId, GetListModel<EventFilterModel> getListModel)
        {
            var query = _dbContext.Events
                .AsNoTracking()
                .Include(x => x.TeamEvents)
                    .ThenInclude(x => x.Team)
                        .ThenInclude(x => x.Users)
                .AsQueryable();

            if (getListModel.Filter != null)
            {
                if (getListModel.Filter.Ids != null)
                    query = query.Where(x => getListModel.Filter.Ids.Contains(x.Id));

                if (!string.IsNullOrWhiteSpace(getListModel.Filter.Name))
                    query = query.Where(x => x.Name.ToLower().Contains(getListModel.Filter.Name));

                if (getListModel.Filter.Statuses != null)
                    query = query.Where(x => getListModel.Filter.Statuses.Contains(x.Status));
                
                if (getListModel.Filter.ExcludeOtherUsersDraftedEvents)
                    query = query.Where(x =>
                        !(x.UserId != userId && x.Status == EventStatus.Draft));

                if (getListModel.Filter.StartFrom.HasValue)
                {
                    var startFrom = getListModel.Filter.StartFrom.Value.ToUtcWithoutSeconds();
                    query = query.Where(x => x.Start.Date >= startFrom);
                }

                if (getListModel.Filter.StartTo.HasValue)
                {
                    var startTo = getListModel.Filter.StartTo.Value.ToUtcWithoutSeconds();
                    query = query.Where(x => x.Start <= startTo);
                }
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getListModel.SortBy))
            {
                query = getListModel.SortBy switch
                {
                    nameof(EventEntity.Name) => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name),

                    nameof(EventEntity.Start) => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Start)
                        : query.OrderByDescending(x => x.Start),

                    nameof(EventEntity.Status) => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Status)
                        : query.OrderByDescending(x => x.Status),

                    _ => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

            var eventModels = await query
                .Skip((getListModel.Page - 1) * getListModel.PageSize)
                .Take(getListModel.PageSize)
                .ProjectToType<EventModel>(_mapper.Config)
                .ToListAsync();

            return new BaseCollectionModel<EventModel>
            {
                Items = eventModels,
                TotalCount = totalCount
            };
        }

        public async Task<EventModel[]> GetByExpression(Expression<Func<EventEntity, bool>> expression)
        {
            return await _dbContext.Events.Where(expression)
                .Where(expression)
                .ProjectToType<EventModel>(_mapper.Config)
                .ToArrayAsync();
        }

        /// <inheritdoc cref="IEventRepository.UpdateAsync(UpdateEventModel)"/>
        public async Task UpdateAsync(UpdateEventModel updateEventModel)
        {
            var eventForUpdate = _mapper.Map<EventEntity>(updateEventModel);
            _dbContext.Events.Update(eventForUpdate);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc cref="IEventRepository.SetStatusAsync(long, EventStatus)"/>
        public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
        {
            var eventEntity = await _dbContext.Events.SingleOrDefaultAsync(x => x.Id == eventId);

            if (eventEntity == null)
                throw new EntityNotFoundException("Событие с указанным идентификатором не найдено");

            eventEntity.Status = eventStatus;
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc cref="IEventRepository.DeleteAsync(long)"/>
        public async Task DeleteAsync(long eventId)
        {
            var eventEntity = await _dbContext.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == eventId);

            _dbContext.Remove(eventEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}