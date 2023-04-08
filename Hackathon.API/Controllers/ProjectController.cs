using Hackathon.Common.Abstraction.Project;
using System.Threading.Tasks;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
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
    /// Получить проект
    /// </summary>
    /// <returns></returns>
    [HttpGet("{eventId:long}/{teamId:long}")]
    public Task<ProjectModel> Get([FromRoute] long eventId, [FromRoute] long teamId)
        => _projectService.GetAsync(eventId, teamId);

    /// <summary>
    /// Создать новый проект
    /// </summary>
    /// <param name="projectCreateRequest">Параметры проекта</param>
    /// <returns></returns>
    [HttpPost]
    public Task<IActionResult> Create([FromBody] ProjectCreateRequest projectCreateRequest)
    {
        var projectCreateModel = _mapper.Map<ProjectCreateParameters>(projectCreateRequest);
        return GetResult(() => _projectService.CreateAsync(projectCreateModel));
    }

    /// <summary>
    /// Обновить проект
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    [HttpPut]
    public Task<IActionResult> Update([FromBody] ProjectUpdateParameters parameters)
        => GetResult(() => _projectService.UpdateAsync(parameters, AuthorizedUserId));

    /// <summary>
    /// Обновить проект из ветки Git-репозитория
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("branch")]
    public Task<IActionResult> UpdateProjectFromGitBranch([FromBody] UpdateProjectFromGitBranchRequest request)
    {
        var parameters = _mapper.Map<UpdateProjectFromGitBranchRequest, UpdateProjectFromGitBranchParameters>(request);
        return GetResult(() => _projectService.UpdateProjectFromGitBranchAsync(parameters, AuthorizedUserId));
    }

    /// <summary>
    /// Удалить проект
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId:long}/{teamId:long}")]
    public Task<IActionResult> Delete(long eventId, long teamId)
        => GetResult(() => _projectService.DeleteAsync(eventId, teamId, AuthorizedUserId));
}
