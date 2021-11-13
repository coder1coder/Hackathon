using System.Threading.Tasks;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction
{
    public interface IEventRepository
    {
        Task<long> CreateAsync(EventModel eventModel);
        Task<EventModel> GetAsync(long eventId);
        Task UpdateAsync(EventModel eventModel);
        Task DeleteAsync(long eventId);
    }
}