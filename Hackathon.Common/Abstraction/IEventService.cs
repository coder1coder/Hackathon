using System.Threading.Tasks;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction
{
    public interface IEventService
    {
        Task<long> CreateAsync(CreateEventModel createEventModel);
        Task SetStatusAsync(long eventId, EventStatus eventStatus);
        Task DeleteAsync(long eventId);
    }
}