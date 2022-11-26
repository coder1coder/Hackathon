using System.Threading.Tasks;
using Hackathon.Abstraction.EventLog;
using Hackathon.Common.Models;
using Hackathon.Common.Models.EventLog;
using Hackathon.Common.Models.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Hackathon.API.Controllers;

[SwaggerTag("Журнал событий")]
public sealed class EventLogController: BaseController
{
    private readonly IEventLogService _service;

    public EventLogController(IEventLogService service)
    {
        _service = service;
    }

    /// <summary>
    /// Получить записи журнала событий
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [HttpPost("list")]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    public async Task<IActionResult> GetListAsync(GetListParameters<EventLogModel> parameters)
    {
        var collectionModel = await _service.GetListAsync(parameters);
        return Ok(collectionModel);
    }
}
