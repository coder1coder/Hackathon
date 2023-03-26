using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Hackathon.API.Controllers.Team;

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
    [ProducesResponseType(typeof(BaseCreateResponse), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> Create(CreateTeamRequest createTeamRequest)
    {
        var createTeamModel = _mapper.Map<CreateTeamModel>(createTeamRequest);

        var createTeamResult = await _teamService.CreateAsync(createTeamModel, AuthorizedUserId);

        if (!createTeamResult.IsSuccess)
            return await GetResult(() => Task.FromResult(createTeamResult));

        return Ok(new BaseCreateResponse
        {
            Id = createTeamResult.Data
        });
    }

    /// <summary>
    /// Получить команду по идентификатору
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TeamModel), (int)HttpStatusCode.OK)]
    public Task<IActionResult> Get([FromRoute] long id)
        => GetResult(() => _teamService.GetAsync(id));

    /// <summary>
    /// Получить все команды
    /// </summary>
    [HttpPost("getTeams")]
    [ProducesResponseType(typeof(BaseCollection<TeamModel>), (int)HttpStatusCode.OK)]
    public async Task<BaseCollection<TeamModel>> GetList([FromBody] GetListParameters<TeamFilter> listRequest)
    {
        var getFilterModel = _mapper.Map<GetListParameters<TeamFilter>>(listRequest);
        return await _teamService.GetListAsync(getFilterModel);
    }

    /// <summary>
    /// Получить общую информацию команды в которой состоит авторизованный пользователь
    /// </summary>
    /// <returns></returns>
    [HttpGet("my")]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(TeamGeneral), (int) HttpStatusCode.OK)]
    public Task<IActionResult> GetUserTeam()
        => GetResult(() => _teamService.GetUserTeam(AuthorizedUserId));

    /// <summary>
    /// Получить события в которых участвовала команда
    /// </summary>
    /// <returns></returns>
    [HttpPost("{teamId:long}/events")]
    [ProducesResponseType(typeof(BaseCollection<TeamEventListItem>), (int) HttpStatusCode.OK)]
    public Task<IActionResult> GetTeamEvents([FromRoute] long teamId,
        [FromBody] PaginationSort paginationSort)
        => GetResult(() => _teamService.GetTeamEvents(teamId, paginationSort));

    /// <summary>
    /// Покинуть команду
    /// </summary>
    /// <returns></returns>
    [HttpGet("{teamId:long}/leave")]
    public Task<IActionResult> LeaveTeamAsync([FromRoute] long teamId)
        => GetResult(() => _teamService.RemoveMemberAsync(new TeamMemberModel
        {
            TeamId = teamId,
            MemberId = AuthorizedUserId
        }));
}
