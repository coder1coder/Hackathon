using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.ApprovalApplications;

public interface IApprovalApplicationRepository
{
    /// <summary>
    /// Удалить заявку на согласование
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор</param>
    /// <returns></returns>
    Task RemoveAsync(long approvalApplicationId);
}
