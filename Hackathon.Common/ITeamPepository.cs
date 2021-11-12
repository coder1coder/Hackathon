using System.Threading.Tasks;
using Hackathon.Common.Entities;

namespace Hackathon.Common
{
    public interface ITeamRepository
    {
        Task<long> CreateAsync(TeamEntity teamEntity);
    }
}