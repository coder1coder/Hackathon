using System.Threading.Tasks;
using Hackathon.Common.Entities;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Abstraction
{
    public interface ITeamRepository
    {
        Task<long> CreateAsync(TeamEntity teamEntity);
        Task<bool> CanCreateAsync(TeamModel teamModel);
    }
}