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
}
