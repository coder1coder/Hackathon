using System.Threading.Tasks;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Abstraction
{
    public interface ITeamService
    {
        Task<long> CreateAsync(CreateTeamModel createTeamModel);
        Task AddMemberAsync(long teamId, long userId);
        Task<TeamModel> GetAsync(long teamId);
    }
}