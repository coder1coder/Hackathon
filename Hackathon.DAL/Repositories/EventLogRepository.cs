using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.EventLog;
using Hackathon.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class EventLogRepository: IEventLogRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventLogRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(EventLogModel eventLogModel)
    {
        var entity = _mapper.Map<EventLogModel, EventLogEntity>(eventLogModel);

        if (entity.UserId.HasValue)
        {
            var userEntity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == entity.UserId);

            if (userEntity is not null)
                entity.UserName = userEntity.GetAnyName();
        }

        _dbContext.EventLog.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BaseCollection<EventLogListItem>> GetListAsync(GetListParameters<EventLogModel> parameters)
    {
        var query = _dbContext.EventLog
            .AsNoTracking();

        var totalCount = await query.LongCountAsync();

        return new BaseCollection<EventLogListItem>
        {
            TotalCount = totalCount,
            Items = await query.Skip(parameters.Offset)
                .Take(parameters.Limit)
                .ProjectToType<EventLogListItem>(_mapper.Config)
                .OrderByDescending(x=>x.Timestamp)
                .ToArrayAsync()
        };
    }
}
