using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Abstraction
{
    public interface ITeamService
    {
        Task<long> CreateAsync(TeamModel teamModel);
    }
}