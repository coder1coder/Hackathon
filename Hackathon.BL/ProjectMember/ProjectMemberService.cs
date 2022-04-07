using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.ProjectMember;

namespace Hackathon.BL.ProjectMember;

public class ProjectMemberService: IProjectMemberService
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IValidator<CreateProjectMemberModel> _createProjectMemberModelValidator;
    private readonly IValidator<ProjectMemberModel> _projectMemberModelValidator;

    public ProjectMemberService(
        IProjectMemberRepository projectMemberRepository,
        IValidator<CreateProjectMemberModel> createProjectMemberModelValidator,
        IValidator<ProjectMemberModel> projectMemberModelValidator)
    {
        _createProjectMemberModelValidator = createProjectMemberModelValidator;
        _projectMemberModelValidator = projectMemberModelValidator;
        _projectMemberRepository = projectMemberRepository;
    }
    public async Task<long> CreateAsync(CreateProjectMemberModel createProjectMemberModel)
    {
        await _createProjectMemberModelValidator.ValidateAndThrowAsync(createProjectMemberModel);
        return await _projectMemberRepository.CreateAsync(createProjectMemberModel);
    }

    public async Task<ProjectMemberModel> GetAsync(long projectMemberId)
    {
        return await _projectMemberRepository.GetAsync(projectMemberId);
    }
}