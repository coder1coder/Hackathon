using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction.ApprovalApplications;

public interface IApprovalApplicationRepository
{
    /// <summary>
    /// Удалить заявку на согласование
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор</param>
    /// <returns></returns>
    Task RemoveAsync(long approvalApplicationId);

    /// <summary>
    /// Получить список заявок на согласование
    /// </summary>
    /// <param name="parameters">Параметры фильтрации и пагинации</param>
    /// <returns></returns>
    Task<Page<ApprovalApplicationModel>> GetListAsync(GetListParameters<ApprovalApplicationFilter> parameters);

    /// <summary>
    /// Получить заявку на согласование по идентификатору заявки
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    /// <returns></returns>
    Task<ApprovalApplicationModel> GetAsync(long approvalApplicationId);

    /// <summary>
    /// Обновить статус заявки на согласование
    /// </summary>
    /// <param name="signerId">Идентификатор подписанта</param>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    /// <param name="status">Статус</param>
    /// <param name="comment">Комментарий к решению</param>
    /// <returns></returns>
    Task UpdateStatusAsync(long signerId, long approvalApplicationId, ApprovalApplicationStatus status,
        string comment = null);
}
