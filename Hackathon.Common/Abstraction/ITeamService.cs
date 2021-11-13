using System.Threading.Tasks;
using Hackathon.Common.Models;

namespace Hackathon.Common.Abstraction
{
    public interface ITeamService
    {
        Task<long> CreateAsync(CreateTeamModel createTeamModel);
    }
}