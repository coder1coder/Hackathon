using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories
{
    public class EventRepository: IEventRepository
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
        public async Task<long> CreateAsync(CreateEventModel createEventModel)
        {
            var eventEntity = _mapper.Map<EventEntity>(createEventModel);

            await _dbContext.AddAsync(eventEntity);
            await _dbContext.SaveChangesAsync();

            return eventEntity.Id;
        }

        public async Task<EventModel> GetAsync(long eventId)
        {
            var eventEntity = await _dbContext.Events
                .AsNoTracking()
                .Include(x=>x.Teams)
                .FirstOrDefaultAsync(x => x.Id == eventId);

            return _mapper.Map<EventModel>(eventEntity);
        }

        public async Task<BaseCollectionModel<EventModel>> GetAsync(GetFilterModel<EventFilterModel> getFilterModel)
        {
            var query = _dbContext.Events.AsNoTracking();

            if (getFilterModel.Filter != null)
            {
                if (getFilterModel.Filter.Id.HasValue)
                    query = query.Where(x => x.Id == getFilterModel.Filter.Id);

                if (!string.IsNullOrWhiteSpace(getFilterModel.Filter.Name))
                    query = query.Where(x => x.Name == getFilterModel.Filter.Name);

                if (getFilterModel.Filter.Status.HasValue)
                    query = query.Where(x => x.Status == getFilterModel.Filter.Status);

                if (getFilterModel.Filter.StartFrom.HasValue)
                {
                    var startFrom = getFilterModel.Filter.StartFrom.Value.Date;
                    query = query.Where(x => x.Start >= startFrom);
                }

                if (getFilterModel.Filter.StartTo.HasValue)
                {
                    var startTo = getFilterModel.Filter.StartTo.Value.Date.AddDays(1);
                    query = query.Where(x => x.Start < startTo);
                }
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getFilterModel.SortBy))
            {
                query = getFilterModel.SortBy switch
                {
                    nameof(EventEntity.Name) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name),

                    nameof(EventEntity.Start) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Start)
                        : query.OrderByDescending(x => x.Start),

                    nameof(EventEntity.Status) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Status)
                        : query.OrderByDescending(x => x.Status),

                    _ => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

            var page = getFilterModel.Page ?? 1;
            var pageSize = getFilterModel.PageSize ?? 1000;

            var eventModels = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<EventModel>()
                .ToListAsync();

            return new BaseCollectionModel<EventModel>
            {
                Items = eventModels,
                TotalCount = totalCount
            };
        }

        public async Task UpdateAsync(EventModel eventModel)
        {
            var eventEntity = _mapper.Map<EventEntity>(eventModel);
            _dbContext.Update(eventEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(long eventId)
        {
            var eventEntity = await _dbContext.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == eventId);

            _dbContext.Remove(eventEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(long eventId)
        {
            return await _dbContext.Events
                .AsNoTracking()
                .AnyAsync(x => x.Id == eventId);
        }
    }
}