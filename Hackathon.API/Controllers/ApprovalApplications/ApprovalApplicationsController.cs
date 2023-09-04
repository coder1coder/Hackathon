using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Models.ApprovalApplications;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers.ApprovalApplications;

[SwaggerTag("Заявления на согласование")]
public class ApprovalApplicationsController: BaseController
{
    private readonly IApprovalApplicationService _approvalApplicationService;

    public ApprovalApplicationsController(IApprovalApplicationService approvalApplicationService)
    {
        _approvalApplicationService = approvalApplicationService;
    }

    /// <summary>
    /// Получить список заявок на согласование
    /// </summary>
    /// <param name="parameters">Параметры фильтрации и пагинации</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IActionResult> GetList([FromQuery] GetListParameters<ApprovalApplicationFilter> parameters)
        => GetResult(() => _approvalApplicationService.GetListAsync(AuthorizedUserId, parameters));
}
