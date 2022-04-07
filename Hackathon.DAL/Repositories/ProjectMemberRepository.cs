using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models.ProjectMember;
using Hackathon.Common.Models.Team;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class ProjectMemberRepository : IProjectMemberRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;

    public ProjectMemberRepository(IMapper mapper, ApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(CreateProjectMemberModel createProjectMemberModel)
    {
        var createProjectMemberEntity = _mapper.Map<ProjectMemberEntity>(createProjectMemberModel);
        
        await _dbContext.AddAsync(createProjectMemberEntity);
        await _dbContext.SaveChangesAsync();
        
        return createProjectMemberEntity.Id;
    }

    public async Task<ProjectMemberModel> GetAsync(long projectMemberId)
    {
        var projectMemberEntity = await _dbContext.ProjectMembers
            .AsNoTracking()
            .Include(x=>x.Project)
            .Include(x=>x.User)
            .SingleOrDefaultAsync(x=>x.Id == projectMemberId);

        if (projectMemberEntity == null)
            throw new Exception("Участник проекта с таким идентификатором не найден");

        return _mapper.Map<ProjectMemberModel>(projectMemberEntity);
    }
    

    public async Task AddProjectMemberRoleAsync(ProjectMemberModel projectMemberModel, string role)
    {
        _dbContext.ChangeTracker.Clear();

        var projectMemberEntity = await _dbContext.ProjectMembers
            .SingleOrDefaultAsync(x=>x.Id == projectMemberModel.ProjectMemberId);

        if (projectMemberEntity == null)
            throw new Exception("Участник проекта с таким идентификатором не найден");

        projectMemberEntity.ProjectMemberRoles.Add(role);

        await _dbContext.SaveChangesAsync();
    }
    

}