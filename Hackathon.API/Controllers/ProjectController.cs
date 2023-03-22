using Hackathon.Common.Abstraction.Project;
using System.Threading.Tasks;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[SwaggerTag("Проекты (результаты событий)")]
public class ProjectController: BaseController
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ProjectController(
        IMapper mapper,
        IProjectService projectService
    )
    {
        _mapper = mapper;
        _projectService = projectService;
    }

    /// <summary>
    /// Получить проект по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    public Task<ProjectModel> Get(long id)
        => _projectService.GetAsync(id);

    /// <summary>
    /// Создать новый проект
    /// </summary>
    /// <param name="projectCreateRequest">Параметры проекта</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<BaseCreateResponse> Create(ProjectCreateRequest projectCreateRequest)
    {
        var projectCreateModel = _mapper.Map<ProjectCreateParameters>(projectCreateRequest);
        var projectId = await _projectService.CreateAsync(projectCreateModel);
        return new BaseCreateResponse
        {
            Id = projectId
        };
    }

    /// <summary>
    /// Обновить проект из гит репозитория
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    [HttpPut("git")]
    public async Task UpdateFromGit(ProjectUpdateFromGitParameters parameters)
    {
        await _projectService.UpdateFromGitAsync(AuthorizedUserId, parameters);
    }
}
