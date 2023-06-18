using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Models.Event;
using Hackathon.DAL.Entities.Event;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories.Events;

public class EventAgreementRepository: IEventAgreementRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventAgreementRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<EventAgreementModel> GetByEventId(long eventId)
    {
        var entity = await _dbContext.EventAgreements
            .Include(x=>x.Event)
            .Include(x=>x.Users)
            .FirstOrDefaultAsync(x => x.EventId == eventId);

        return entity is not null
            ? _mapper.Map<EventAgreementEntity, EventAgreementModel>(entity)
            : null;
    }

    public async Task UpsertUserRelationAsync(long eventId, long userId)
    {
        var hasRelation = await _dbContext.EventAgreementUsers
            .AnyAsync(x => x.AgreementId == eventId && x.UserId == userId);

        if (hasRelation)
            return;

        _dbContext.EventAgreementUsers.Add(new EventAgreementUserEntity
        {
            AgreementId = eventId,
            UserId = userId
        });

        await _dbContext.SaveChangesAsync();
    }
}
