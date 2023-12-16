using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Logbook.Abstraction.Models;
using Hackathon.Logbook.Abstraction.Repositories;
using Hackathon.Logbook.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Logbook.DAL.Repositories;

public class EventLogRepository: IEventLogRepository
{
    private readonly LogbookDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventLogRepository(LogbookDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(EventLogModel eventLogModel)
    {
        var entity = _mapper.Map<EventLogModel, EventLogEntity>(eventLogModel);
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
