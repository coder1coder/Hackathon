using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction
{
    public interface IEventRepository
    {
        Task<long> CreateAsync(CreateEventModel createEventModel);
        Task<EventModel> GetAsync(long eventId);
        Task<BaseCollectionModel<EventModel>> GetAsync(GetFilterModel<EventFilterModel> getFilterModel);
        Task UpdateAsync(EventModel eventModel);
        Task DeleteAsync(long eventId);
        Task<bool> ExistAsync(long eventId);
        Task<bool> ExistAsync(long[] eventsIds);
    }
}