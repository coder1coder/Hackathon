using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction.ApprovalApplications;

public interface IApprovalApplicationRepository
{
    /// <summary>
    /// Добавить заявку на согласование
    /// </summary>
    /// <param name="parameters">Параметры</param>
    Task AddAsync(NewApprovalApplicationParameters parameters);

    /// <summary>
    /// Удалить заявку на согласование
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор</param>
    Task RemoveAsync(long approvalApplicationId);

    /// <summary>
    /// Получить список заявок на согласование
    /// </summary>
    /// <param name="parameters">Параметры фильтрации и пагинации</param>
    Task<Page<ApprovalApplicationModel>> GetListAsync(GetListParameters<ApprovalApplicationFilter> parameters);

    /// <summary>
    /// Получить заявку на согласование по идентификатору заявки
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    Task<ApprovalApplicationModel> GetAsync(long approvalApplicationId);

    /// <summary>
    /// Обновить статус заявки на согласование
    /// </summary>
    /// <param name="signerId">Идентификатор подписанта</param>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    /// <param name="status">Статус</param>
    /// <param name="comment">Комментарий к решению</param>
    Task UpdateStatusAsync(long signerId, long approvalApplicationId, ApprovalApplicationStatus status,
        string comment = null);
}
