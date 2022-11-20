using System.Net;
using System.Threading.Tasks;
using Hackathon.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[SwaggerTag("Команды")]
public class TeamController : BaseController
{
    private readonly IMapper _mapper;
    private readonly ITeamService _teamService;

    /// <summary>
    /// Команды
    /// </summary>
    public TeamController(
        ITeamService teamService, IMapper mapper)
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
    public async Task<TeamModel> Get([FromRoute] long id)
        => await _teamService.GetAsync(id);

    /// <summary>
    /// Получить все команды
    /// </summary>
    [HttpPost("getTeams")]
    [ProducesResponseType(typeof(BaseCollectionResponse<TeamModel>), (int)HttpStatusCode.OK)]
    public async Task<BaseCollectionResponse<TeamModel>> GetList([FromBody] GetListParameters<TeamFilter> listRequest)
    {
        var getFilterModel = _mapper.Map<GetListParameters<TeamFilter>>(listRequest);
        var collectionModel = await _teamService.GetAsync(getFilterModel);
        return _mapper.Map<BaseCollectionResponse<TeamModel>>(collectionModel);
    }

    /// <summary>
    /// Получить общую информацию команды в которой состоит авторизованный пользователь
    /// </summary>
    /// <returns></returns>
    [HttpGet("my")]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(TeamGeneral), (int) HttpStatusCode.OK)]
    public async Task<TeamGeneral> GetUserTeam()
        => await _teamService.GetUserTeam(UserId);

    /// <summary>
    /// Получить события в которых участвовала команда
    /// </summary>
    /// <returns></returns>
    [HttpPost("{teamId:long}/events")]
    [ProducesResponseType(typeof(BaseCollectionResponse<TeamEventListItem>), (int) HttpStatusCode.OK)]
    public async Task<BaseCollectionResponse<TeamEventListItem>> GetTeamEvents([FromRoute] long teamId,
        [FromBody] PaginationSort paginationSort)
    {
        var collection = await _teamService.GetTeamEvents(teamId, paginationSort);

        return new BaseCollectionResponse<TeamEventListItem>
        {
            Items = collection.Items,
            TotalCount = collection.TotalCount
        };
    }

    /// <summary>
    /// Покинуть команду
    /// </summary>
    /// <returns></returns>
    [HttpGet("{teamId:long}/leave")]
    public async Task<IActionResult> LeaveTeamAsync(long teamId)
    {
        var teamMember = new TeamMemberModel
        {
            TeamId = teamId,
            MemberId = UserId
        };

        await _teamService.RemoveMemberAsync(teamMember);
        return Ok();
    }
}
