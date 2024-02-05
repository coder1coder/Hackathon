using System.Net;
using Hackathon.Common.Abstraction.Project;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[SwaggerTag("Проекты (результаты событий)")]
public class ProjectController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IProjectService _projectService;

    public ProjectController(IMapper mapper,
        IProjectService projectService)
    {
        _mapper = mapper;
        _projectService = projectService;
    }

    /// <summary>
    /// Получить проект
    /// </summary>
    /// <returns></returns>
    [HttpGet("{eventId:long}/{teamId:long}")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(ProjectModel))]
    public Task<IActionResult> Get([FromRoute] long eventId, [FromRoute] long teamId)
        => GetResult(() => _projectService.GetAsync(AuthorizedUserId, eventId, teamId));

    /// <summary>
    /// Создать новый проект
    /// </summary>
    /// <param name="projectCreateRequest">Параметры проекта</param>
    /// <returns></returns>
    [HttpPost]
    public Task<IActionResult> Create([FromBody] ProjectCreateRequest projectCreateRequest)
    {
        var projectCreateModel = _mapper.Map<ProjectCreationParameters>(projectCreateRequest);
        return GetResult(() => _projectService.CreateAsync(AuthorizedUserId, projectCreateModel));
    }

    /// <summary>
    /// Обновить проект
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    [HttpPut]
    public Task<IActionResult> Update([FromBody] ProjectUpdateParameters parameters)
        => GetResult(() => _projectService.UpdateAsync(AuthorizedUserId, parameters));

    /// <summary>
    /// Обновить проект из ветки Git-репозитория
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("branch")]
    public Task<IActionResult> UpdateProjectFromGitBranch([FromBody] UpdateProjectFromGitBranchRequest request)
    {
        var parameters = _mapper.Map<UpdateProjectFromGitBranchRequest, UpdateProjectFromGitBranchParameters>(request);
        return GetResult(() => _projectService.UpdateProjectFromGitBranchAsync(AuthorizedUserId, parameters));
    }

    /// <summary>
    /// Удалить проект
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId:long}/{teamId:long}")]
    public Task<IActionResult> Delete(long eventId, long teamId)
        => GetResult(() => _projectService.DeleteAsync(AuthorizedUserId, eventId, teamId));
}
