using System.Threading.Tasks;
using Hackathon.Common;
using Hackathon.Common.Entities;

namespace Hackathon.DAL.Repositories
{
    public class EventRepository: IEventRepository
    {
        public Task<long> CreateAsync(EventEntity eventEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}