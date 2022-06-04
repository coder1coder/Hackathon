using System.Threading.Tasks;
using Hackathon.Abstraction.Project;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[SwaggerTag("Проекты (результаты событий)")]
public class ProjectController: BaseController, IProjectApi
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
    /// Создать новый проект
    /// </summary>
    /// <param name="projectCreateRequest">Параметры проекта</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<BaseCreateResponse> Create(ProjectCreateRequest projectCreateRequest)
    {
        var projectCreateModel = _mapper.Map<ProjectCreateModel>(projectCreateRequest);
        var projectId = await _projectService.CreateAsync(projectCreateModel);
        return new BaseCreateResponse
        {
            Id = projectId
        };
    }
}