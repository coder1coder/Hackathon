using System.Threading.Tasks;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Abstraction
{
    public interface ITeamRepository
    {
        Task<long> CreateAsync(CreateTeamModel createTeamModel);
        Task<TeamModel> GetAsync(long teamId);
        Task<bool> ExistAsync(string teamName);
        Task<bool> ExistAsync(long teamId);
        Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel);
    }
}