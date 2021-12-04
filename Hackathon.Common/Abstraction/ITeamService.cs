using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Abstraction
{
    public interface ITeamService
    {
        Task<long> CreateAsync(CreateTeamModel createTeamModel);
        Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel);
        Task<TeamModel> GetAsync(long teamId);
        Task<BaseCollectionModel<TeamModel>> GetAsync(GetFilterModel<TeamFilterModel> getFilterModel);
    }
}