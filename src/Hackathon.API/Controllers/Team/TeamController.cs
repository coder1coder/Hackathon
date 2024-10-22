using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Contracts;
using Hackathon.API.Contracts.Teams;
using Hackathon.API.Module;
using Hackathon.Common.Models.Teams;

namespace Hackathon.API.Controllers.Team;

/// <summary>
/// Команды
/// </summary>
[SwaggerTag("Команды")]
public class TeamController : BaseController
{
    private readonly ITeamService _teamService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Команды
    /// </summary>
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
    [ProducesResponseType(typeof(BaseCreateResponse), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> Create(CreateTeamRequest createTeamRequest)
    {
        var createTeamModel = _mapper.Map<CreateTeamModel>(createTeamRequest);

        var createTeamResult = await _teamService.CreateAsync(createTeamModel, AuthorizedUserId);

        if (!createTeamResult.IsSuccess)
        {
            return await GetResult(() => Task.FromResult(createTeamResult));
        }

        return Ok(new BaseCreateResponse
        {
            Id = createTeamResult.Data
        });
    }

    /// <summary>
    /// Получить команду по идентификатору
    /// </summary>
    /// <param name="teamId"></param>
    [HttpGet("{teamId:long}")]
    [ProducesResponseType(typeof(TeamModel), (int)HttpStatusCode.OK)]
    public Task<IActionResult> Get([FromRoute] long teamId)
        => GetResult(() => _teamService.GetAsync(teamId));

    /// <summary>
    /// Получить все команды
    /// </summary>
    [HttpPost("getTeams")]
    [ProducesResponseType(typeof(BaseCollection<TeamModel>), (int)HttpStatusCode.OK)]
    public Task<BaseCollection<TeamModel>> GetList([FromBody] GetListParameters<TeamFilter> listRequest)
        => _teamService.GetListAsync(listRequest);

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
