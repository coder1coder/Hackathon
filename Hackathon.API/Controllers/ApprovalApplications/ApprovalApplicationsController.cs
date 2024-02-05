using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.API.Module;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Models.ApprovalApplications;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers.ApprovalApplications;

[SwaggerTag("Заявления на согласование")]
public class ApprovalApplicationsController : BaseController
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
    [HttpPost("list")]
    public Task<IActionResult> GetList([FromBody] GetListParameters<ApprovalApplicationFilter> parameters)
        => GetResult(() => _approvalApplicationService.GetListAsync(AuthorizedUserId, parameters));

    /// <summary>
    /// Получить заявку на согласование по идентификатору заявки
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    /// <returns></returns>
    [HttpGet("{approvalApplicationId:long}")]
    public Task<IActionResult> Get(long approvalApplicationId)
        => GetResult(() => _approvalApplicationService.GetAsync(AuthorizedUserId, approvalApplicationId));

    /// <summary>
    /// Согласовать заявку на согласование
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    /// <returns></returns>
    [HttpPost("{approvalApplicationId:long}/approve")]
    public Task<IActionResult> Approve(long approvalApplicationId)
        => GetResult(() => _approvalApplicationService.ApproveAsync(AuthorizedUserId, approvalApplicationId));

    /// <summary>
    /// Отклонить заявку на согласование
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    /// <param name="parameters">Параметры решения</param>
    /// <returns></returns>
    [HttpPost("{approvalApplicationId:long}/reject")]
    public Task<IActionResult> Reject(long approvalApplicationId, [FromBody] ApprovalApplicationRejectParameters parameters)
        => GetResult(() => _approvalApplicationService.RejectAsync(AuthorizedUserId, approvalApplicationId, parameters));
}
