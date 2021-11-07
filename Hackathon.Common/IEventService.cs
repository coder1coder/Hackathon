using System.Threading.Tasks;
using Hackathon.Common.Models;

namespace Hackathon.Common
{
    public interface IEventService
    {
        Task<long> CreateAsync(CreateEventModel createEventModel);
    }
}