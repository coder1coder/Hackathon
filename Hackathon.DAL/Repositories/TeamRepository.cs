using System;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
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
        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            var createTeamEntity = _mapper.Map<TeamEntity>(createTeamModel);

            await _dbContext.AddAsync(createTeamEntity);
            await _dbContext.SaveChangesAsync();

            return createTeamEntity.Id;
        }

        public async Task<TeamModel> GetAsync(long teamId)
        {
            var teamEntity = await _dbContext.Teams
                .AsNoTracking()
                .Include(x=>x.Event)
                .Include(x=>x.Users)
                .Include(x=>x.Project)
                .FirstOrDefaultAsync(x => x.Id == teamId);

            if (teamEntity == null)
                throw new Exception("Команда с таким идентификатором не найдена");

            return _mapper.Map<TeamModel>(teamEntity);
        }

        public async Task<bool> ExistAsync(string teamName)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Name.ToLower() == teamName.ToLower());
        }

        public async Task<bool> ExistAsync(long teamId)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Id == teamId);
        }

        public async Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel)
        {
            var teamEntity = await _dbContext.Teams
                .FirstOrDefaultAsync(x=>x.Id == teamAddMemberModel.TeamId);

            if (teamEntity == null)
                throw new Exception("Команда с указаным индентификатором не найдена");

            var userEntity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == teamAddMemberModel.UserId);

            teamEntity.Users.Add(userEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}