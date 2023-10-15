using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Validation.ApprovalApplications;
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

    private const string NoRightsExecutingOperation = "Нет прав на выполнение операции";
    private const string ApprovalApplicationDoesntExist = "Заявка на согласование не найдена";
    private const string ApprovalApplicationAlreadyHasDecision = "По заявке на согласование уже вынесено решение";

    private readonly ApprovalApplicationRejectParametersValidator _rejectParametersValidator;

    public ApprovalApplicationService(
        IApprovalApplicationRepository approvalApplicationRepository,
        IUserRepository userRepository,
        ApprovalApplicationRejectParametersValidator rejectParametersValidator)
    {
        _approvalApplicationRepository = approvalApplicationRepository;
        _userRepository = userRepository;
        _rejectParametersValidator = rejectParametersValidator;
    }

    public async Task<Result<Page<ApprovalApplicationModel>>> GetListAsync(long authorizedUserId, GetListParameters<ApprovalApplicationFilter> parameters)
    {
        var user = await _userRepository.GetAsync(authorizedUserId);

        if (user is not { Role: UserRole.Administrator })
            return Result<Page<ApprovalApplicationModel>>.Forbidden(NoRightsExecutingOperation);

        var pagedApprovalApplications = await _approvalApplicationRepository.GetListAsync(parameters);

        return Result<Page<ApprovalApplicationModel>>.FromValue(pagedApprovalApplications);
    }

    public async Task<Result<ApprovalApplicationModel>> GetAsync(long authorizedUserId, long approvalApplicationId)
    {
        var authorizedUser = await _userRepository.GetAsync(authorizedUserId);
        if (authorizedUser is null)
            return Result<ApprovalApplicationModel>.Forbidden(NoRightsExecutingOperation);

        var approvalApplication = await _approvalApplicationRepository.GetAsync(approvalApplicationId);
        if (approvalApplication is null)
            return Result<ApprovalApplicationModel>.NotFound(ApprovalApplicationDoesntExist);

        if (authorizedUser.Role != UserRole.Administrator || authorizedUser.Id != approvalApplication.AuthorId)
            return Result<ApprovalApplicationModel>.Forbidden(NoRightsExecutingOperation);

        return Result<ApprovalApplicationModel>.FromValue(approvalApplication);
    }

    public async Task<Result> ApproveAsync(long authorizedUserId, long approvalApplicationId)
    {
        var authorizedUser = await _userRepository.GetAsync(authorizedUserId);
        if (authorizedUser is not { Role: UserRole.Administrator })
            return Result.Forbidden(NoRightsExecutingOperation);

        var approvalApplication = await _approvalApplicationRepository.GetAsync(approvalApplicationId);
        if (approvalApplication is null)
            return Result.NotFound(ApprovalApplicationDoesntExist);

        if (approvalApplication.ApplicationStatus is not ApprovalApplicationStatus.Requested)
            return Result.NotValid(ApprovalApplicationAlreadyHasDecision);

        await _approvalApplicationRepository.UpdateStatusAsync(approvalApplicationId, approvalApplicationId,
            ApprovalApplicationStatus.Approved);

        return Result.Success;
    }

    public async Task<Result> RejectAsync(long authorizedUserId, long approvalApplicationId, ApprovalApplicationRejectParameters parameters)
    {
        await _rejectParametersValidator.ValidateAndThrowAsync(parameters);

        var authorizedUser = await _userRepository.GetAsync(authorizedUserId);
        if (authorizedUser is not { Role: UserRole.Administrator })
            return Result.Forbidden(NoRightsExecutingOperation);

        var approvalApplication = await _approvalApplicationRepository.GetAsync(approvalApplicationId);
        if (approvalApplication is null)
            return Result.NotFound(ApprovalApplicationDoesntExist);

        if (approvalApplication.ApplicationStatus is not ApprovalApplicationStatus.Requested)
            return Result.NotValid(ApprovalApplicationAlreadyHasDecision);

        await _approvalApplicationRepository.UpdateStatusAsync(approvalApplicationId, approvalApplicationId,
            ApprovalApplicationStatus.Approved, parameters.Comment);

        return Result.Success;
    }
}
