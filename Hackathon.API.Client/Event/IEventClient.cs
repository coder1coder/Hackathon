using System.Threading.Tasks;
using Hackathon.Common.Models.Event;

namespace Hackathon.API.Client.Event
{
    public interface IEventClient
    {
        Task<EventModel> Get(long id);
    }
}