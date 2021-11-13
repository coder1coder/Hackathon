using System.Threading.Tasks;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;

namespace Hackathon.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        public Task<long> CreateAsync(TeamEntity teamEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}