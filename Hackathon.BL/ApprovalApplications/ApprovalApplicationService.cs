using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.ApprovalApplications;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.ApprovalApplications;

public class ApprovalApplicationService: IApprovalApplicationService
{
    private readonly IApprovalApplicationRepository _approvalApplicationRepository;
    private readonly IUserRepository _userRepository;

    public ApprovalApplicationService(
        IApprovalApplicationRepository approvalApplicationRepository,
        IUserRepository userRepository)
    {
        _approvalApplicationRepository = approvalApplicationRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<Page<ApprovalApplicationModel>>> GetListAsync(long authorizedUserId, GetListParameters<ApprovalApplicationFilter> parameters)
    {
        var user = await _userRepository.GetAsync(authorizedUserId);

        if (user is not { Role: UserRole.Administrator })
            return Result<Page<ApprovalApplicationModel>>.Forbidden("Нет прав на выполнение операции");

        var pagedApprovalApplications = await _approvalApplicationRepository.GetListAsync(parameters);

        return Result<Page<ApprovalApplicationModel>>.FromValue(pagedApprovalApplications);
    }
}
