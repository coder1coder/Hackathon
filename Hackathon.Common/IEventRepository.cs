using System.Threading.Tasks;
using Hackathon.Common.Entities;

namespace Hackathon.Common
{
    public interface IEventRepository
    {
        Task<long> CreateAsync(EventEntity eventEntity);
    }
}