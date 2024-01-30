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
public class ProjectController(
    IMapper mapper,
    IProjectService projectService) : BaseController
{
    /// <summary>
    /// Получить проект
    /// </summary>
    /// <returns></returns>
    [HttpGet("{eventId:long}/{teamId:long}")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(ProjectModel))]
    public Task<IActionResult> Get([FromRoute] long eventId, [FromRoute] long teamId)
        => GetResult(() => projectService.GetAsync(AuthorizedUserId, eventId, teamId));

    /// <summary>
    /// Создать новый проект
    /// </summary>
    /// <param name="projectCreateRequest">Параметры проекта</param>
    /// <returns></returns>
    [HttpPost]
    public Task<IActionResult> Create([FromBody] ProjectCreateRequest projectCreateRequest)
    {
        var projectCreateModel = mapper.Map<ProjectCreationParameters>(projectCreateRequest);
        return GetResult(() => projectService.CreateAsync(AuthorizedUserId, projectCreateModel));
    }

    /// <summary>
    /// Обновить проект
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    [HttpPut]
    public Task<IActionResult> Update([FromBody] ProjectUpdateParameters parameters)
        => GetResult(() => projectService.UpdateAsync(AuthorizedUserId, parameters));

    /// <summary>
    /// Обновить проект из ветки Git-репозитория
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("branch")]
    public Task<IActionResult> UpdateProjectFromGitBranch([FromBody] UpdateProjectFromGitBranchRequest request)
    {
        var parameters = mapper.Map<UpdateProjectFromGitBranchRequest, UpdateProjectFromGitBranchParameters>(request);
        return GetResult(() => projectService.UpdateProjectFromGitBranchAsync(AuthorizedUserId, parameters));
    }

    /// <summary>
    /// Удалить проект
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpDelete("{eventId:long}/{teamId:long}")]
    public Task<IActionResult> Delete(long eventId, long teamId)
        => GetResult(() => projectService.DeleteAsync(AuthorizedUserId, eventId, teamId));
}
