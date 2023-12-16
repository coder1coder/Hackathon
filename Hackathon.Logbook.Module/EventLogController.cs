using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Hackathon.Logbook.Abstraction.Models;
using Hackathon.Logbook.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.Logbook.Module;

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
        => Ok(await _service.GetListAsync(parameters));
}
