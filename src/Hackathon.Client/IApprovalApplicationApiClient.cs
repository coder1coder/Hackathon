using System.Threading.Tasks;
using Refit;

namespace Hackathon.Client;

public interface IApprovalApplicationApiClient
{
    private const string BaseRoute = "/api/ApprovalApplications";

    /// <summary>
    /// Согласовать заявку на согласование
    /// </summary>
    /// <param name="approvalApplicationId">Идентификатор заявки</param>
    [Post(BaseRoute + "/{approvalApplicationId}/approve")]
    Task Approve(long approvalApplicationId);
}
