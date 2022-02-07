using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    public class TeamController : BaseController, ITeamApi
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
        [HttpPost]
        public async Task<BaseCreateResponse> Create(CreateTeamRequest createTeamRequest)
        {
            var createTeamModel = _mapper.Map<CreateTeamModel>(createTeamRequest);

            //attach user who creates it
            createTeamModel.OwnerId = UserId;
            var teamId = await _teamService.CreateAsync(createTeamModel);
            return new BaseCreateResponse
            {
                Id = teamId,
            };
        }

        /// <summary>
        /// Получить команду по идентификатору
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id:long}")]
        public Task<TeamModel> Get([FromRoute] long id)
        {
            return _teamService.GetAsync(id);
        }

        /// <summary>
        /// Получить все команды
        /// </summary>
        [HttpPost("getTeams")]
        public async Task<BaseCollectionResponse<TeamModel>> Get([FromBody] GetListRequest<TeamFilterModel> listRequest)
        {
            var getFilterModel = _mapper.Map<GetListModel<TeamFilterModel>>(listRequest);
            var collectionModel = await _teamService.GetAsync(getFilterModel);
            return _mapper.Map<BaseCollectionResponse<TeamModel>>(collectionModel);
        }
    }
}