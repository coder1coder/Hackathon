using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        public TeamRepository(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<long> CreateAsync(TeamEntity teamModel)
        {
            var teamEntity = _mapper.Map<TeamEntity>(teamModel);

            await _dbContext.AddAsync(teamEntity);
            await _dbContext.SaveChangesAsync();

            return teamEntity.Id;
        }

        public async Task<bool> CanCreateAsync(TeamModel teamModel)
        {
            var query = _dbContext.Teams
                .AsNoTracking()
                .Where(x => x.Name.ToLower() == teamModel.Name.ToLower());

            return query == null;
        }
    }
}