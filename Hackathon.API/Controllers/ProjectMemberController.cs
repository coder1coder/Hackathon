using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models.ProjectMember;
using Hackathon.Contracts.Requests.ProjectMember;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

public class ProjectMemberController: BaseController, IProjectMemberApi
{
    private readonly IMapper _mapper;
    private readonly IProjectMemberService _projectMemberService;

    public ProjectMemberController(
        IProjectMemberService projectMemberService, IMapper mapper)
    {
        _projectMemberService = projectMemberService;
        _mapper = mapper;
    }

    /// <summary>
    /// Создание нового участника команды
    /// </summary>
    /// <param name="createProjectMemberRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<BaseCreateResponse> Create(CreateProjectMemberRequest createProjectMemberRequest)
    {
        var createProjectMemberModel = _mapper.Map<CreateProjectMemberModel>(createProjectMemberRequest);
        var projectMemberId = await _projectMemberService.CreateAsync(createProjectMemberModel);
        return new BaseCreateResponse
        {
            Id = projectMemberId,
        };
    }
    
    /// <summary>
    /// Получить участника команды по идентификатору
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:long}")]
    public Task<ProjectMemberModel> Get([FromRoute] long id)
    {
        return _projectMemberService.GetAsync(id);
    }
}