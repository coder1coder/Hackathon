using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Models.Event;
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
        public async Task<long> CreateAsync(EventModel eventModel)
        {
            var eventEntity = _mapper.Map<EventEntity>(eventModel);

            await _dbContext.AddAsync(eventEntity);
            await _dbContext.SaveChangesAsync();

            return eventEntity.Id;
        }

        public async Task<EventModel> GetAsync(long eventId)
        {
            var eventEntity = await _dbContext.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == eventId);

            return _mapper.Map<EventModel>(eventEntity);
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
    }
}