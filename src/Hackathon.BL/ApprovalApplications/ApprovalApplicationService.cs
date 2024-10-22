using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Validation.Users;
using Hackathon.Common.Abstraction.ApprovalApplications;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Users;
using Hackathon.Informing.Abstractions.Models.Notifications.Data;
using Hackathon.Informing.Abstractions.Services;
using Hackathon.Informing.BL;

namespace Hackathon.BL.ApprovalApplications;

public class ApprovalApplicationService: IApprovalApplicationService
{
    private readonly IApprovalApplicationRepository _approvalApplicationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<ApprovalApplicationRejectParameters> _rejectParametersValidator;
    private readonly INotificationService _notificationService;

    public ApprovalApplicationService(
        IApprovalApplicationRepository approvalApplicationRepository,
        IUserRepository userRepository,
        IValidator<ApprovalApplicationRejectParameters> rejectParametersValidator,
        INotificationService notificationService)
    {
        _approvalApplicationRepository = approvalApplicationRepository;
        _userRepository = userRepository;
        _rejectParametersValidator = rejectParametersValidator;
        _notificationService = notificationService;
    }

    /// <inheritdoc />
    public async Task<Result<Page<ApprovalApplicationModel>>> GetListAsync(long authorizedUserId, GetListParameters<ApprovalApplicationFilter> parameters)
    {
        var user = await _userRepository.GetAsync(authorizedUserId);

        if (user is not { Role: UserRole.Administrator })
        {
            return Result<Page<ApprovalApplicationModel>>.Forbidden(ApprovalApplicationMessages.NoRightsExecutingOperation);
        }

        var pagedApprovalApplications = await _approvalApplicationRepository.GetListAsync(parameters);

        return Result<Page<ApprovalApplicationModel>>.FromValue(pagedApprovalApplications);
    }

    /// <inheritdoc />
    public async Task<Result<ApprovalApplicationModel>> GetAsync(long authorizedUserId, long approvalApplicationId)
    {
        var authorizedUser = await _userRepository.GetAsync(authorizedUserId);
        if (authorizedUser is null)
        {
            return Result<ApprovalApplicationModel>.NotFound(UserValidationErrorMessages.UserDoesNotExists);
        }

        var approvalApplication = await _approvalApplicationRepository.GetAsync(approvalApplicationId);
        if (approvalApplication is null)
        {
            return Result<ApprovalApplicationModel>.NotFound(ApprovalApplicationMessages.ApprovalApplicationDoesntExist);
        }

        if (authorizedUser.Id != approvalApplication.AuthorId)
        {
            return Result<ApprovalApplicationModel>.Forbidden(ApprovalApplicationMessages.NoRightsExecutingOperation);
        }

        return Result<ApprovalApplicationModel>.FromValue(approvalApplication);
    }

    public async Task<Result> ApproveAsync(long authorizedUserId, long approvalApplicationId)
    {
        var authorizedUser = await _userRepository.GetAsync(authorizedUserId);
        if (authorizedUser is not { Role: UserRole.Administrator })
        {
            return Result.Forbidden(ApprovalApplicationMessages.NoRightsExecutingOperation);
        }

        var approvalApplication = await _approvalApplicationRepository.GetAsync(approvalApplicationId);
        if (approvalApplication is null)
        {
            return Result.NotFound(ApprovalApplicationMessages.ApprovalApplicationDoesntExist);
        }

        if (approvalApplication.ApplicationStatus is not ApprovalApplicationStatus.Requested)
        {
            return Result.NotValid(ApprovalApplicationMessages.ApprovalApplicationAlreadyHasDecision);
        }

        await _approvalApplicationRepository.UpdateStatusAsync(authorizedUserId, approvalApplicationId,
            ApprovalApplicationStatus.Approved);

        await _notificationService.PushAsync(NotificationCreator.System(new SystemNotificationData("Заявка на согласование одобрена"),
            approvalApplication.AuthorId, 
            authorizedUserId));

        return Result.Success;
    }

    public async Task<Result> RejectAsync(long authorizedUserId, long approvalApplicationId, ApprovalApplicationRejectParameters parameters)
    {
        await _rejectParametersValidator.ValidateAndThrowAsync(parameters);

        var authorizedUser = await _userRepository.GetAsync(authorizedUserId);
        if (authorizedUser is not { Role: UserRole.Administrator })
        {
            return Result.Forbidden(ApprovalApplicationMessages.NoRightsExecutingOperation);
        }

        var approvalApplication = await _approvalApplicationRepository.GetAsync(approvalApplicationId);
        if (approvalApplication is null)
        {
            return Result.NotFound(ApprovalApplicationMessages.ApprovalApplicationDoesntExist);
        }

        if (approvalApplication.ApplicationStatus is not ApprovalApplicationStatus.Requested)
        {
            return Result.NotValid(ApprovalApplicationMessages.ApprovalApplicationAlreadyHasDecision);
        }

        await _approvalApplicationRepository.UpdateStatusAsync(authorizedUserId, approvalApplicationId,
            ApprovalApplicationStatus.Rejected, parameters.Comment);

        await _notificationService.PushAsync(NotificationCreator
            .System(new SystemNotificationData($"Заявка на согласование отклонена:\n{parameters.Comment}"),
            approvalApplication.AuthorId,
            authorizedUserId));

        return Result.Success;
    }
}
