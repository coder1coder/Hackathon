using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction
{
    public interface IEventService
    {
        Task<long> CreateAsync(CreateEventModel createEventModel);
        Task<EventModel> GetAsync(long eventId);
        Task<BaseCollectionModel<EventModel>> GetAsync(GetFilterModel<EventFilterModel> getFilterModel);
        Task SetStatusAsync(long eventId, EventStatus eventStatus);
        Task DeleteAsync(long eventId);
    }
}