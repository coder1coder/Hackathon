using System.Threading.Tasks;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.Team;
using Microsoft.AspNetCore.Authorization;

namespace Hackathon.API.Controllers
{
    // [ApiController]
    // [Route("[controller]")]
    public class TeamController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService, IMapper mapper)
        {
            _teamService = teamService;
            _mapper = mapper;
        }

        /// <summary>
        /// Создание новой команды
        /// </summary>
        /// <param name="createTeamRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<BaseCreateResponse> Create(CreateTeamRequest createTeamRequest)
        {
            var teamModel = _mapper.Map<TeamModel>(createTeamRequest);
            var teamId = await _teamService.CreateAsync(teamModel);
            return new BaseCreateResponse
            {
                Id = teamId,
            };
        }
    }
}