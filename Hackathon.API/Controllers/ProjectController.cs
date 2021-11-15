using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
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

        [Authorize]
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
}